using Common;
using Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Service;
using System;
using System.Data.Entity;

namespace Web
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(EFDbContext.Create);
            app.CreatePerOwinContext<UserService>(UserService.Create);
            app.CreatePerOwinContext<RoleService>(RoleService.Create);
            app.CreatePerOwinContext<SignInService>(SignInService.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<UserService, User, int>(
                        validateInterval: TimeSpan.FromHours(2),
                        regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                    getUserIdCallback: (id) => (id.GetUserId<int>()))
                },
                ExpireTimeSpan = TimeSpan.FromHours(2),
                AuthenticationMode = AuthenticationMode.Active//Force Owin to refresh
            });
        }
    }
}