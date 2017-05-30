using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebApi.Providers;
using Service;
using Data;
using WebApi.Models;
using System.Web;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;

namespace WebApi
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            //ef上下文
            app.CreatePerOwinContext(EFDbContext.Create);
            app.CreatePerOwinContext<UserService>(UserService.Create);
            app.CreatePerOwinContext<RoleService>(RoleService.Create);
            app.CreatePerOwinContext<SignInService>(SignInService.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            //init Bearer Auth
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(7),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true,
                Provider = new SimpleAuthorizationServerProvider(PublicClientId),
                AccessTokenFormat = new TicketDataFormat(app.CreateDataProtector(
               typeof(OAuthAuthorizationServerMiddleware).Namespace,
               "Access_Token", "v1")),
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
            //set AccessTokenFormat
            OAuthBearerOptions.AccessTokenFormat = OAuthOptions.AccessTokenFormat;
        }
    }
}
