using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using WebApi.Models;
using Service;
using Microsoft.Owin;
using Autofac.Integration.WebApi;
using Newtonsoft.Json.Linq;
using Microsoft.Owin.Security.OAuth;
using Common.Utilities;
using Common;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    [AutofacControllerConfiguration]
    public class AccountController : ApiController
    {
        #region Ctor

        private const string LocalLoginProvider = "Local";
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private SignInService _signInService;
        private ISettingService _settingService;
        private IAttachmentService _attachmentService;
        private IAuthenticationManager _authenticationService;

        public AccountController(
            IOwinContext context,
            ISettingService settingService,
            IAttachmentService attachmentService)
        {
            this._userService = context.GetUserManager<UserService>();
            this._roleService = context.Get<RoleService>();
            this._signInService = context.Get<SignInService>();
            this._settingService = settingService;
            this._attachmentService = attachmentService;
            this._authenticationService = context.Authentication;
        }

        #endregion

        #region 用户逻辑方法

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model">用户注册model</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new MessageData(false, "实体数据验证错误", ModelState));
            }
            string encryptPassword = string.Empty;
            string encryptPasswordComfirm = string.Empty;
            try
            {
                encryptPassword = EncryptionHelper.Base64_Decode(model.Password);
                encryptPasswordComfirm = EncryptionHelper.Base64_Decode(model.Password);
            }
            catch (Exception ex)
            {
                return Ok(new MessageData(false, "用户名或秘密不正确"));
            }

            if (!encryptPassword.Equals(encryptPasswordComfirm))
            {
                return Ok(new MessageData(false, "密码和确定密码不一致"));
            }
            if (encryptPassword.Length < 6 || encryptPasswordComfirm.Length < 6)
                return Ok(new MessageData(false, "请输入至少6位数的密码"));
            if (_userService.PhoneNumberIsExists(model.PhoneNumber.Trim()))
                return Ok(new MessageData(false, "改手机号已存在"));

            IdentityResult result;
            JObject resObj;

            var user = new Common.User();
            user.TrueName = string.Empty;
            user.UserName = model.PhoneNumber;
            user.Password = encryptPassword;
            user.PhoneNumber = model.PhoneNumber;
            user.Description = string.Empty;
            user.UserType = UserType.Customer;
            user.IsDelete = false;
            user.CreateTime = DateTime.Now;

            //创建用户
            result = await _userService.CreateAsync(user, encryptPassword);
            if (result.Succeeded)
            {
                result = await _userService.AddToRolesAsync(user.Id, "客户");
            }
            if (!result.Succeeded)
            {
                IHttpActionResult errorResult = GetErrorResult(result);
                if (errorResult != null)
                {
                    return errorResult;
                }
            }
            resObj = GenerateLocalAccessTokenResponse(model.PhoneNumber);//生成用户返回信息tokne和过期时间etc

            return Ok(new MessageData(true, "注册成功", resObj));
        }

        /// <summary>
        /// web api portal页面登陆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("WebPortalLogin")]
        public async Task<IHttpActionResult> WebPortalLogin(webLoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var getUser = await _userService.FindAsync(model.username, model.password);
            if (getUser != null)
            {
                _signInService.SetCurrentUser(getUser);
                await _signInService.SignInAsync(getUser, isPersistent: false, rememberBrowser: false);

                return Ok("/swagger");
            }
            return Ok("success");
        }


        // POST api/Account/ChangePassword
        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ModifyPassword")]
        public async Task<IHttpActionResult> ModifyPassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new MessageData(false, "修改密码失败", ModelState));
            }
            var oldPassword = EncryptionHelper.Base64_Decode(model.OldPassword);
            var newPassword = EncryptionHelper.Base64_Decode(model.NewPassword);
            var comfirmPassword = EncryptionHelper.Base64_Decode(model.ConfirmPassword);
            if (oldPassword.Equals(newPassword))
                return Ok(new MessageData(false, "新密码不能与旧密码相同"));
            var authUser = await _userService.FindByIdAsync(int.Parse(User.Identity.GetUserId()));
            if (authUser.UserType == UserType.Admin)
                return Ok(new MessageData(false, "系统用户不能通过api修改"));

            IdentityResult result = await _userService.ChangePasswordAsync(authUser.Id, oldPassword,
                newPassword);

            if (!result.Succeeded)
            {
                return Ok(new MessageData(false, "修改密码失败", result));
            }

            return Ok(new MessageData(true, "修改密码成功"));
        }

        // POST api/Account/SetPassword
        /// <summary>
        /// 设置密码/忘记密码
        /// </summary>
        /// <param name="model">设定密码Model</param>
        /// <returns></returns>
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new MessageData(false, "修改密码失败", ModelState));
            }
            var authUserId = int.Parse(User.Identity.GetUserId());
            //根据用户Id获取Owin token
            var token = await _userService.GeneratePasswordResetTokenAsync(authUserId);
            var result = await _userService.ResetPasswordAsync(authUserId, token, model.NewPassword);

            if (!result.Succeeded)
            {
                return Ok(new MessageData(false, "修改密码失败", result));
            }

            return Ok(new MessageData(true, "修改密码成功"));
        }

        /// <summary>
        /// 上传用户头像
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("UploadAvatar")]
        public async Task<IHttpActionResult> UploadAvatar()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count != 1)
                return Ok(new MessageData(false, "请选择一张图片上传"));
            #region Field

            //文件流
            var postFile = httpRequest.Files[0];
            var uploadMaxSetting = _settingService.FindSettingByName("上传文件最大值(M)");
            int maxContentLength = 1024 * 1024 * Convert.ToInt16(uploadMaxSetting.Number);
            int authUserId = int.Parse(User.Identity.GetUserId());
            var authUser = await _userService.FindByIdAsync(authUserId);
            var apiHost = _settingService.FindSettingByName("api物理地址");

            #endregion
            //允许上传的格式
            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
            var ext = postFile.FileName.Substring(postFile.FileName.LastIndexOf('.'));
            var extension = ext.ToLower();
            if (!AllowedFileExtensions.Contains(extension))
            {
                var message = string.Format("请上传 .jpg,.gif,.png格式的图片");
                return Ok(new MessageData(false, message));
            }
            //图片大小限制
            if (postFile.ContentLength > maxContentLength)
                return Ok(new MessageData(false, "文件上传最大为" + Convert.ToInt16(uploadMaxSetting.Number) + " M"));
            Attachment imgEntity;
            try
            {
                imgEntity = new Attachment();
                imgEntity.CreatedTime = DateTime.Now;
                imgEntity.ContentType = postFile.ContentType;
                imgEntity.FileName = postFile.FileName;
                imgEntity.OriginalName = StringHelper.GenerateOriginName(postFile.FileName, imgEntity.CreatedTime);
                imgEntity.FileLength = postFile.ContentLength;
                imgEntity.TenantType = UserType.Customer;
                imgEntity.UserId = authUserId;

                _attachmentService.Create(imgEntity, postFile.InputStream);
                authUser.AvatarId = imgEntity.Id;
                await _userService.UpdateAsync(authUser);
            }
            catch (Exception ex)
            {
                return Ok(new MessageData(false, "上传头像失败"));
            }

            return Ok(new MessageData(true, "上传头像成功", new { avatarPath = apiHost.Code + imgEntity.FilePath }));
        }

        /// <summary>
        /// 密码解密
        /// </summary>
        /// <param name="hashedPassword">加密过的密码串</param>
        /// <returns></returns>
        [HttpGet]
        [Route("BaseDecode")]
        public string BaseDecode(string hashedPassword)
        {
            return EncryptionHelper.Base64_Decode(hashedPassword);
        }

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="password">要加密的密码串</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("BaseEncode")]
        public string BaseEncode(string password)
        {
            return EncryptionHelper.Base64_Encode(password);
        }

        // GET api/values
        /// <summary>
        /// 获取当前用户名称
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetPrincipal")]
        public string GetPrincipal()
        {
            var userName = this.RequestContext.Principal.Identity.Name;
            return String.Format("Hello, {0} :)", userName);
        }

        /// <summary>
        /// 用户实名验证
        /// </summary>
        /// <param name="model">实名验证实体</param>
        /// <returns></returns>
        [Route("Verification")]
        public async Task<IHttpActionResult> Verification(VerificationModel model)
        {
            if (!ModelState.IsValid)
                return Ok(new MessageData(false, "身份验证失败", ModelState));

            int authUserId = int.Parse(User.Identity.GetUserId());
            var authUser = await _userService.FindByIdAsync(authUserId);
            if (authUser.IsTrueName)
                return Ok(new MessageData(false, "您的身份验证已成功", ModelState));
            authUser.TrueName = model.TrueName;
            authUser.IsTrueName = false;
            authUser.IdNumber = model.IdentityCode;
            try
            {
                await _userService.UpdateAsync(authUser);
            }
            catch (Exception ex)
            {
                return Ok(new MessageData(false, "身份验证提交失败"));
            }

            return Ok(new MessageData(true, "身份验证成功"));
        }

        /// <summary>
        /// 修改性别
        /// </summary>
        /// <param name="model">性别类型</param>
        /// <returns></returns>
        [Route("ModifyGender")]
        public async Task<IHttpActionResult> ModifyGender(ModifyGenderModel model)
        {
            if (model.Gender <= 0)
                return Ok(new MessageData(false, "性别输入有误"));
            int authUserId = int.Parse(User.Identity.GetUserId());
            var authUser = await _userService.FindByIdAsync(authUserId);

            if (model.Gender == 1)
                authUser.Gender = GenderType.Female;
            if (model.Gender == 2)
                authUser.Gender = GenderType.Male;
            if (model.Gender == 3)
                authUser.Gender = GenderType.Undetermined;
            try
            {
                await _userService.UpdateAsync(authUser);
            }
            catch (Exception ex)
            {
                return Ok(new MessageData(false, "修改性别失败"));
            }

            return Ok(new MessageData(true, "修改性别成功", authUser.Gender));
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="mdoel">昵称</param>
        /// <returns></returns>
        [Route("ModifyNickName")]
        public async Task<IHttpActionResult> ModifyNickName(ModifyNickNameModel mdoel)
        {
            if (String.IsNullOrEmpty(mdoel.NickName))
                return Ok(new MessageData(false, "请输入昵称"));
            int authUserId = int.Parse(User.Identity.GetUserId());
            var authUser = await _userService.FindByIdAsync(authUserId);

            authUser.NickName = mdoel.NickName;
            try
            {
                await _userService.UpdateAsync(authUser);
            }
            catch (Exception ex)
            {
                return Ok(new MessageData(false, "修改昵称失败"));
            }

            return Ok(new MessageData(true, "修改昵称成功", authUser.NickName));
        }

        /// <summary>
        /// 获取设备正常图标
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetNormalIcon")]
        public string GetNormalIcon()
        {
            var apiHost = _settingService.FindSettingByName("api物理地址");
            return apiHost.Code + "/Content/Images/normal.png";
        }

        /// <summary>
        /// 获取设备已占用图标
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetOccupyIcon")]
        public string GetOccupyIcon()
        {
            var apiHost = _settingService.FindSettingByName("api物理地址");
            return apiHost.Code + "/Content/Images/occupy.png";
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("PersonalInfo")]
        public async Task<IHttpActionResult> PersonalInfo()
        {
            int authUserId = int.Parse(User.Identity.GetUserId());
            var authUser = await _userService.FindByIdAsync(authUserId);
            var apiHost = _settingService.FindSettingByName("api物理地址");
            if (authUser.PhoneNumber.Length != 11)
                return Ok(new MessageData(false, "用户手机格式错误"));
            string replaceStr = authUser.PhoneNumber.Substring(3, 4);
            string phoneEncrypt = authUser.PhoneNumber.Replace(replaceStr, "****");//选择显示手机号
            string verifyMessage = authUser.IsTrueName ? "已认证" : "未认证";//验证结果
            string avatarUrl = authUser.AvatarId.HasValue ? apiHost.Code + authUser.AvatarTable.FilePath : "";

            return Ok(new MessageData(true, "获取用户信息成功", new { phone = phoneEncrypt, trueName = (authUser.IsTrueName ? authUser.TrueName : ""), verifyMsg = verifyMessage, balance = authUser.CurrentAmount, avatarUrl = avatarUrl }));
        }

        #endregion

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        [NonAction]
        private JObject GenerateLocalAccessTokenResponse(string userName)
        {
            //过期时间
            var tokenExpiration = TimeSpan.FromDays(7);
            var user = _userService.FindByTrueName(userName);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("role", "客户"));

            var props = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = string.Empty;
            try
            {
                accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);
            }
            catch (Exception ex)
            {
                _userService.Delete(user);
                throw ex;
            }

            JObject tokenResponse = new JObject(
                                        new JProperty("userName", userName),
                                        new JProperty("access_token", accessToken),
                                        new JProperty("token_type", "bearer"),
                                        new JProperty("expires_seconds", tokenExpiration.TotalSeconds.ToString()),
                                        new JProperty(".issued_utc_time", ticket.Properties.IssuedUtc.ToString()),
                                        new JProperty(".expires_utc_time", ticket.Properties.ExpiresUtc.ToString())
        );

            return tokenResponse;
        }

        #endregion
    }
}
