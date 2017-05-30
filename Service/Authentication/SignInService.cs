using Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// 用户登陆逻辑
    /// </summary>
    public class SignInService : SignInManager<User, int>
    {
        public SignInService(UserService userManager, IAuthenticationManager authenticationManager) : 
            base(userManager, authenticationManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((UserService)UserManager);
        }

        public static SignInService Create(IdentityFactoryOptions<SignInService> options, IOwinContext context)
        {
            return new SignInService(context.GetUserManager<UserService>(), context.Authentication);
        }

        private User _signInUser;

        public void SetCurrentUser(User user)
        {
            _signInUser = user;
        }

        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        /// <returns></returns>
        public User GetCurrentUser()
        {
            if (_signInUser != null)
                return _signInUser;

            var id = AuthenticationManager.User.Identity.GetUserId<int>();

            _signInUser = UserManager.FindById(id);
            return _signInUser;
        }

        /// <summary>
        /// 登出
        /// </summary>
        public void SignOut()
        {
            _signInUser = null;
        }
    }
}
