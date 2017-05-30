using Common;
using Service;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Web
{
    /// <summary>
    /// 用户扩展
    /// </summary>
    public static class UserExtension
    {
        /// <summary>
        /// 判断用户是否属于某个角色
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="roleName">角色名称</param>
        /// <param name="isAdmin">是否是超级管理员</param>
        /// <returns></returns>
        public static bool HasRole(this User user, string roleName, bool? isAdmin = null)
        {
            if (user == null)
                return false;
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<UserService>();
            if (!isAdmin.HasValue)
            {
                if (userManager.IsInRole(user.Id, "超级管理员"))
                    return true;
            }
            return userManager.IsInRole(user.Id, roleName);
        }

        /// <summary>
        /// 判断用户是否拥有某个权限
        /// </summary>
        public static bool HasPermission(this User user, string permissionName)
        {
            if (user == null)
                return false;

            if (user.HasRole("超级管理员"))
                return true;

            if (string.IsNullOrEmpty(permissionName))
                return true;

            var permissionService = DIContainer.Resolve<IPermissionService>();

            var permissions = user.Roles.SelectMany(n => permissionService.GetRolePermissions(n.RoleId));
            foreach (var p in permissions)
            {
                if (p.Name == permissionName)
                    return true;
            }

            var permission = permissionService.FindByName(permissionName);
            if (permission != null && user.Permissions.Where(n => n.PermissionId == permission.Id).Count() > 0)
                return true;

            return false;
        }
    }
}