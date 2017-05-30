using Common;
using Service;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Web
{
    /// <summary>
    /// 用户上下文
    /// </summary>
    public class UserContext
    {
        /// <summary>
        /// 获取当前用户
        /// </summary>
        public static User CurrentUser
        {
            get
            {
                var user = HttpContext.Current.GetOwinContext().Get<SignInService>().GetCurrentUser();
                return user;
            }
        }

        /// <summary>
        /// 获取当前用户Id
        /// </summary>
        public static int CurrentUserId
        {
            get
            {
                return HttpContext.Current.User.Identity.GetUserId<int>();
            }
        }
    }
}