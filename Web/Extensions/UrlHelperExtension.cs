using Common;
using Common.Utilities;
using Service;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Web
{
    public static class UrlHelperExtension
    {
        /// <summary>
        /// 站点首页
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        public static string Home(this UrlHelper urlHelper)
        {
            return urlHelper.Action("Index", "Admin");
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public static string LogIn(this UrlHelper urlHelper, string returnUrl = null)
        {
            return urlHelper.Action("Login", "Account", new { returnUrl = returnUrl });
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        public static string LogOut(this UrlHelper urlHelper, string returnUrl = null)
        {
            return urlHelper.Action("LogOut", "Account", new { returnUrl = returnUrl });
        }

        /// <summary>
        /// 校长验证
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        public static string SchoolValidate(this UrlHelper urlHelper)
        {
            return urlHelper.Action("SchoolValidate", "Account");
        }

        /// <summary>
        /// 代理商验证
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        public static string AgentValidate(this UrlHelper urlHelper)
        {
            return urlHelper.Action("AgentValidate", "Account");
        }

        /// <summary>
        /// 校长验证
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        public static string ProjectValidate(this UrlHelper urlHelper)
        {
            return urlHelper.Action("ProjectValidate", "Account");
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string UserHead(this UrlHelper urlHelper, int userId, bool isOriginal = false)
        {
            string original = isOriginal ? "_original" : "";

            var userService = DIContainer.Resolve<IOwinContext>().GetUserManager<UserService>();

            string filePath = urlHelper.RequestContext.HttpContext.Server.MapPath("~/Upload/Avatar/" + userId + ".jpg");
            if (System.IO.File.Exists(filePath))
                return WebHelper.ResolveUrl("~/Upload/Avatar/" + userId + original + ".jpg");

            var claims = userService.GetClaims(userId);
            if (claims != null)
            {
                var claim = claims.Where(n => n.Type == "avatar").FirstOrDefault();
                if (claim != null)
                {
                    return claim.Value;
                }
            }

            return WebHelper.ResolveUrl("~/Content/img/userhead.jpg");
        }

        /// <summary>
        /// 学校Logo
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        //public static string SchoolLogo(this UrlHelper urlHelper, int schoolId)
        //{
        //    var userService = DIContainer.Resolve<IOwinContext>().GetUserManager<UserService>();

        //    string filePath = urlHelper.RequestContext.HttpContext.Server.MapPath("~/Upload/Logo/School" + schoolId + ".jpg");
        //    if (System.IO.File.Exists(filePath))
        //        return WebHelper.ResolveUrl("~/Upload/Logo/School" + schoolId + ".jpg");

        //    return WebHelper.ResolveUrl("~/Content/img/School_house.png");
        //}

        /// <summary>
        /// 获取图片路径
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static string ImageUrl(this UrlHelper urlHelper, Attachment attachment)
        {
            var attachmentService = DIContainer.Resolve<IAttachmentService>();
            return attachmentService.GetRelativePath(attachment);
        }

        /// <summary>
        /// 创建、编辑角色
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string EditRole(this UrlHelper urlHelper, int? id = null)
        {
            return urlHelper.Action("EditRole", "Admin", new { id = id });
        }

        /// <summary>
        /// 菜单权限分配
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ManageDistributionPermission(this UrlHelper urlHelper, int? id = null)
        {
            return urlHelper.Action("ManageDistributionPermission", "Admin", new { id = id });
        }

        /// <summary>
        /// 页签权限分配
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ManageDistributionTabs(this UrlHelper urlHelper, int? id = null)
        {
            return urlHelper.Action("ManageDistributionTabs", "Admin", new { id = id });
        }
        
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteRole(this UrlHelper urlHelper, int? id = null)
        {
            return urlHelper.Action("DeleteRole", "Admin", new { id = id });
        }

        /// <summary>
        /// 添加用户到指定角色
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static string AddUserToRole(this UrlHelper urlHelper, int roleId)
        {
            return urlHelper.Action("AddUserToRole", "Admin", new { roleId = roleId });
        }

        /// <summary>
        /// 添加权限到指定角色
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static string AddPermissionToRole(this UrlHelper urlHelper, int roleId)
        {
            return urlHelper.Action("AddPermissionToRole", "Admin", new { roleId = roleId });
        }

        /// <summary>
        /// 创建、编辑权限
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string EditPermission(this UrlHelper urlHelper, int? id = null)
        {
            return urlHelper.Action("EditPermission", "Admin", new { id = id });
        }

        #region 系统管理

        /// <summary>
        /// 创建用户
        /// </summary>
        public static string EditUser(this UrlHelper urlHelper, int? id = null)
        {
            return urlHelper.Action("EditUser", "Admin", new { id = id });
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        public static string DeleteUser(this UrlHelper urlHelper, int? id = null)
        {
            return urlHelper.Action("DeleteUser", "Admin", new { id = id });
        }

        /// <summary>
        /// 冻结用户
        /// </summary>
        public static string FreezeUser(this UrlHelper urlHelper, int? id = null)
        {
            return urlHelper.Action("FreezeUser", "Admin", new { id = id });
        }

        /// <summary>
        /// 授权用户
        /// </summary>
        public static string RefreezeUser(this UrlHelper urlHelper, int? id = null)
        {
            return urlHelper.Action("RefreezeUser", "Admin", new { id = id });
        }

        /// <summary>
        /// 回收站恢复
        /// </summary>
        public static string RollBack(this UrlHelper urlHelper, int? id = null)
        {
            return urlHelper.Action("RollBack", "Admin", new { id = id });
        }

        /// <summary>
        /// 编辑任务
        /// </summary>
        public static string EditTask(this UrlHelper urlHelper, int? id = null)
        {
            return urlHelper.Action("EditTask", "Admin", new { id = id });
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        public static string DeleteTask(this UrlHelper urlHelper, int? id = null)
        {
            return urlHelper.Action("DeleteTask", "Admin", new { id = id });
        }

        #endregion

        #region 设备

        /// <summary>
        /// 根据地区code获取省市区的名称
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public static string GetProvinceAndCity(this UrlHelper urlHelper)
        {
            return urlHelper.Action("GetProvinceAndCity", "Device");
        }

        #endregion

        /// <summary>
        /// 删除学员
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        public static string DeleteStudent(this UrlHelper urlHelper)
        {
            return urlHelper.Action("DeleteStudent", "Market");
        }

    }
}
