using Common;
using Service;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Common.Utilities;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Web.UI;
using System.Collections.Generic;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Ctor

        private UserService _userService;
        private RoleService _roleService;
        private SignInService _signInService;
        private IAuthenticationManager _authenticationService;

        public AccountController(IOwinContext context, IQuartzTaskService quartzTaskService, ILogger logger)
        {
            this._userService = context.GetUserManager<UserService>();
            this._roleService = context.Get<RoleService>();
            this._signInService = context.Get<SignInService>();
            this._authenticationService = context.Authentication;
        }

        #endregion

        #region 登陆√

        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult Login(string returnUrl = "")
        {
            if (Request.Browser.IsMobileDevice)
                return RedirectToAction("MobileLogin", new { returnUrl = returnUrl });
            return View(new LoginEditModel() { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// 处理登陆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginEditModel model)
        {
            string modelName = Request.Browser.IsMobileDevice ? "MobileLogin" : "Login";


            if (!ModelState.IsValid)
                return View(modelName, model);
            string message = string.Empty;
            var userAdjust = _userService.FindByTrueName(model.UserName);
            if (userAdjust == null)
            {
                AddError("用户名或密码错误");
                return View(modelName, model);
            }
            if (userAdjust.IsBlackUser == true)
            {
                AddError("该用户已被列为黑名单");
            }
            else
            {
                if (string.IsNullOrEmpty(model.Password))
                {
                    AddError("请输入密码");
                    return View(modelName, model);
                }
                var getUser = await _userService.FindAsync(model.UserName, model.Password);
                //var result = await _userService.AddLoginAsync(getUser.Id, new UserLoginInfo("loginProvider", "providerKey"));//通过User添加登录信息
                if (getUser != null)
                {
                    _signInService.SetCurrentUser(getUser);
                    await _signInService.SignInAsync(getUser, isPersistent: false, rememberBrowser: false);

                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Request.ApplicationPath != model.ReturnUrl)
                        return Redirect(WebHelper.ResolveUrl("~/" + model.ReturnUrl));
                    return RedirectToAction("Index", "Admin");
                }
                AddError("用户名或密码错误");
            }
            if (Request.Browser.IsMobileDevice)
            {
                return View(modelName, model);
            }
            return View(modelName, model);
        }
        #endregion

        #region 注册

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email, TrueName = model.Email };
                try
                {
                    var result = await _userService.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _signInService.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                        return RedirectToAction("Index", "Admin");
                    }
                    AddError(result);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region 注销√
        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult LogOut(string returnUrl = null)
        {
            _authenticationService.SignOut();
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region 个人信息

        /// <summary>
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PersonalInfo()
        {
            User user = UserContext.CurrentUser;
            UserEditModel model = user.MapTo<UserEditModel>();
            return View(model);
        }

        /// <summary>
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PersonalInfo(UserEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = UserContext.CurrentUser;
            //if (_userService.FindByTrueName(model.TrueName) != null && user.TrueName != model.TrueName)
            //{
            //    AddError("昵称已存在");
            //    return View(model);
            //}

            //HttpPostedFileBase hpf = Request.Files["UserHead"];
            //string path = Server.MapPath("~/Upload/Avatar/" + user.Id + ".jpg");
            //if (width > 0 && height > 0)
            //{
            //    Image image = Image.FromFile(path);
            //    var newImage = ImageHelper.Crop(image, x, y, width, height);
            //    newImage.Save(path);
            //}
            //else if (hpf != null && hpf.ContentLength > 0)
            //{
            //    Image image = Image.FromStream(hpf.InputStream);
            //    var newImage = ImageHelper.Resize(image, 200, 200);
            //    newImage.Save(path);
            //}

            user.PhoneNumber = model.PhoneNumber;
            user.TrueName = model.TrueName;
            user.Email = model.Email;
            var result = await _userService.UpdateAsync(user);
            ViewBag.Msg = result.Succeeded ? "true" : "false";
            return View(model);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ChangePassword(PasswordEditModel model)
        {
            User user = UserContext.CurrentUser;
            bool checkPassword = await _userService.FindAsync(user.UserName, model.CurrentPassword) == null;
            if (checkPassword)
            {
                ViewBag.Msg = "currenterror";
                return View();
            }
            if (model.CurrentPassword == model.Password)
            {
                ViewBag.Msg = "samepwd";
                return View();
            }
            var result = await _userService.ChangePasswordAsync(user.Id, model.CurrentPassword, model.Password);
            if (result.Succeeded)
            {
                ViewBag.Msg = "true";
                return View();
            }
            else
            {
                ViewBag.Msg = "false";
                return View(model);
            }

        }

        /// <summary>
        /// 修改密码【忘记密码的情况】
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ActionResult ChangePasswordWithoutPassword()
        {
            return View();
        }

        /// <summary>
        /// 修改密码【忘记密码的情况】
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public async Task<ActionResult> ChangePasswordWithoutPassword(ForgetPwdModel model)
        {
            //根据用户Id获取Owin token
            var token = await _userService.GeneratePasswordResetTokenAsync(model.ModifyUserId);
            var result = await _userService.ResetPasswordAsync(model.ModifyUserId, token, model.Password);
            
            if (result.Succeeded)
            {
                ViewBag.Msg = "true";
                return View();
            }
            else
            {
                ViewBag.Msg = "false";
                return View(model);
            }
        }

        #endregion

        #region ExternalLogin

        /// <summary>
        /// 第三方登陆
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        /// <summary>
        /// 第三方登陆回调
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await _authenticationService.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _signInService.ExternalSignInAsync(loginInfo, isPersistent: false);
            if (result == SignInStatus.Success)
            {
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            ViewBag.Avatar = loginInfo.ExternalIdentity.FindFirstValue("urn:QQ:avatar");
            return View("ExternaRegister", new LoginEditModel { NickName = loginInfo.DefaultUserName });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternaRegister(LoginEditModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }

            if (ModelState.IsValid)
            {
                var info = await _authenticationService.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    AddError("认证失败");
                    return RedirectToAction("Login");
                }

                string avatarUrl = info.ExternalIdentity.FindFirstValue("urn:QQ:avatar");
                ViewBag.Avatar = avatarUrl;

                if (_userService.FindByName(model.UserName) != null)
                {
                    AddError("用户名已存在");
                    return View(model);
                }

                if (_userService.FindByTrueName(model.NickName) != null)
                {
                    AddError("昵称已存在");
                    return View(model);
                }

                var user = new User(model.UserName, model.NickName);
                var result = await _userService.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userService.AddClaimAsync(user.Id, new Claim("avatar", avatarUrl));
                    result = await _userService.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await _signInService.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    AddError(result);
                    return View(model);
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            AddError("未知错误");
            return View(model);
        }

        private const string XsrfKey = "XsrfId";

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion

        #region Helpers

        private void AddError(IdentityResult result)
        {
            if (result == null)
                return;
            if (result.Errors != null && result.Errors.Count() > 0)
            {
                AddError(result.Errors.First());
            }
        }

        private void AddError(string message)
        {
            ViewData["errorMessage"] = message;
        }

        #endregion

        #region Test

        [AllowAnonymous]
        public async Task<JsonResult> SendEmail()
        {
            await _userService.SendEmailAsync(1, "邮件测试", "邮件测试正文。。");
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}