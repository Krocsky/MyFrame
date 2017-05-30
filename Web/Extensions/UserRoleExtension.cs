using Common;
using Service;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Web
{
    /// <summary>
    /// 角色扩展
    /// </summary>
    public static class UserRoleExtension
    {
        /// <summary>
        /// 获取角色
        /// </summary>
        public static Role GetRole(this UserRole userRole)
        {
            if (userRole == null)
                return null;
            var roleService = HttpContext.Current.GetOwinContext().Get<RoleService>();
            return roleService.FindById(userRole.RoleId);
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        public static User GetUser(this UserRole userRole)
        {
            if (userRole == null)
                return null;
            var userService = HttpContext.Current.GetOwinContext().GetUserManager<UserService>();
            return userService.FindById(userRole.UserId);
        }
    }
}