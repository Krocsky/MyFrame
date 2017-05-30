using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "index",
                url: "",
                defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional, navid = 101 }
            );

            routes.MapRoute(
               name: "ExternalLogin",
               url: "Account/ExternalLogin",
               defaults: new { controller = "Account", action = "ExternalLogin" }
           );

            routes.MapRoute(
               name: "UserHead",
               url: "Account/UserHead/{userid}",
               defaults: new { controller = "Account", action = "UserHead" }
           );

            routes.MapRoute(
                name: "admin",
                url: "Admin",
                defaults: new { controller = "Admin", action = "Index", navid = 2 }
            );

            #region 导航菜单

            //菜单管理
            routes.MapRoute(
               name: "ManageNavigation",
               url: "Admin/ManageNavigation",
               defaults: new { controller = "Admin", action = "ManageNavigation" }
           );

            //用户管理
            routes.MapRoute(
               name: "ManageUsers",
               url: "Admin/ManageUsers",
               defaults: new { controller = "Admin", action = "ManageUsers" }
           );

            //角色管理
            routes.MapRoute(
               name: "ManageRoles",
               url: "Admin/ManageRoles",
               defaults: new { controller = "Admin", action = "ManageRoles" }
           );

            //角色用户
            routes.MapRoute(
               name: "ManageRoleUsers",
               url: "Admin/ManageRoleUsers",
               defaults: new { controller = "Admin", action = "ManageRoleUsers" }
           );

            //权限管理
            routes.MapRoute(
               name: "ManagePermissions",
               url: "Admin/ManagePermissions",
               defaults: new { controller = "Admin", action = "ManagePermissions" }
           );

            //角色权限
            routes.MapRoute(
               name: "ManageRolePermissions",
               url: "Admin/ManageRolePermissions",
               defaults: new { controller = "Admin", action = "ManageRolePermissions" }
           );

            //权限分配
            routes.MapRoute(
               name: "ManageSchoolPermission",
               url: "School/ManageSchoolPermission",
               defaults: new { controller = "School", action = "ManageSchoolPermission" }
           );

            #endregion

            #region 系统设置

            //系统设置
            routes.MapRoute(
               name: "ManageSetting",
               url: "Setting/ManageSetting",
               defaults: new { controller = "Setting", action = "ManageSetting" }
           );

            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
