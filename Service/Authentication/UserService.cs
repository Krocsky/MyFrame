using Common;
using Common.Utilities;
using Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// 用户业务逻辑
    /// </summary>
    public class UserService : UserManager<User, int>
    {
        public UserService(IUserStore<User, int> store)
            : base(store) { }



        public static UserService Create(IdentityFactoryOptions<UserService> options,
            IOwinContext context)
        {
            var manager = new UserService(DIContainer.Resolve<UserStore>());

            manager.UserValidator = new UserValidator<User, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromSeconds(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            manager.EmailService = new EmailService();

            return manager;
        }

        /// <summary>
        /// 根据昵称查找用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public User FindByTrueName(string name)
        {
            var user = Users.Where(n => n.TrueName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            return user.SingleOrDefault();
        }

        /// <summary>
        /// 根据用户Id集合获取用户列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IList<User> GetUserByIds(IList<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return new List<User>();
            return Users.Where(n => ids.Contains(n.Id)).ToList();
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        public IEnumerable<User> GetUsers
        {
            get { return Users.Where(n => !n.IsDelete); }
        }

        /// <summary>
        /// 创建用户并添加到相应的角色
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        public bool CreateUserWithRole(User user,string role)
        {
            try
            {
                CreateAsync(user, user.Password);
                AddToRoleAsync(user.Id, role);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检查手机号码是否存在
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public bool PhoneNumberIsExists(string phoneNumber)
        {
            if (Users.FirstOrDefault(n => n.PhoneNumber.Equals(phoneNumber, StringComparison.InvariantCultureIgnoreCase)) == null)
                return false;
            return true;
        }

        /// <summary>
        /// 检查登录账户是否存在
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public bool LoginAccountIsExists(string loginName)
        {
            if (Users.FirstOrDefault(u => u.UserName.Equals(loginName, StringComparison.InvariantCultureIgnoreCase)) == null)
                return false;
            return true;
        }
    }
}
