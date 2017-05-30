using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    /// <summary>
    /// 权限验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PermissionCheckAttribute : FilterAttribute, IAuthorizationFilter
    {
        private string permission = "";

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Permission
        {
            get { return this.permission; }
            set { this.permission = value; }
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.IsChildAction)
                return;

            if (!Check())
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult { Data = new MessageData(false, "您没有权限访问此页面") };
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Admin/Index");
                }
            }
        }

        private bool Check()
        {
            User user = UserContext.CurrentUser;
            if (user == null)
                return false;

            if (user.HasPermission(permission))
                return true;

            return false;
        }
    }
}