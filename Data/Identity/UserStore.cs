using Common;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Data
{
    public class UserStore : IUserStore<User, int>, IUserPasswordStore<User, int>, IUserEmailStore<User, int>, IUserLoginStore<User, int>, IUserRoleStore<User, int>, IUserTwoFactorStore<User, int>, IUserClaimStore<User, int>, IQueryableUserStore<User, int>, IUserPhoneNumberStore<User, int>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserLogin, long> _userLoginRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserClaim> _userClaimRepository;

        public UserStore(IRepository<User> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<UserClaim> userClaimRepository)
        {
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userClaimRepository = userClaimRepository;
        }

        public virtual async Task CreateAsync(User user)
        {
            await _userRepository.InsertAsync(user);
        }

        public virtual async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public virtual async Task DeleteAsync(User user)
        {
            await _userRepository.DeleteAsync(user);
        }

        public virtual async Task<User> FindByIdAsync(int userId)
        {
            return await _userRepository.FirstOrDefaultAsync(userId);
        }

        public virtual async Task<User> FindByNameAsync(string userName)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => user.UserName == userName
                );
        }

        public virtual async Task<User> FindByEmailAsync(string email)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => user.Email == email
                );
        }

        public virtual Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }

        public virtual Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.Password);
        }

        public virtual Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.Password));
        }

        public virtual Task SetEmailAsync(User user, string email)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public virtual Task<string> GetEmailAsync(User user)
        {
            return Task.FromResult(user.Email);
        }

        public virtual Task<bool> GetEmailConfirmedAsync(User user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public virtual Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public virtual async Task AddLoginAsync(User user, UserLoginInfo login)
        {
            await _userLoginRepository.InsertAsync(
                new UserLogin
                {
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                    UserId = user.Id
                });
        }

        public virtual async Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            var userId = user.Id;
            var entry = await _userLoginRepository.Table.SingleOrDefaultAsync(l => l.UserId.Equals(userId) && l.LoginProvider == login.LoginProvider && l.ProviderKey == login.ProviderKey);
            if (entry != null)
            {
                await _userLoginRepository.DeleteAsync(entry);
            }
        }

        public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            var userId = user.Id;
            return await _userLoginRepository.Table.Where(l => l.UserId.Equals(userId))
                .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToListAsync();
        }

        public virtual async Task<User> FindAsync(UserLoginInfo login)
        {
            var userLogin = await _userLoginRepository.FirstOrDefaultAsync(
                ul => ul.LoginProvider == login.LoginProvider && ul.ProviderKey == login.ProviderKey
                );

            if (userLogin == null)
            {
                return null;
            }

            return await _userRepository.FirstOrDefaultAsync(u => u.Id == userLogin.UserId);
        }

        public virtual Task<List<User>> FindAllAsync(UserLoginInfo login)
        {
            var query = from userLogin in _userLoginRepository.Table
                        join user in _userRepository.Table on userLogin.UserId equals user.Id
                        where userLogin.LoginProvider == login.LoginProvider && userLogin.ProviderKey == login.ProviderKey
                        select user;

            return Task.FromResult(query.ToList());
        }

        public virtual async Task AddToRoleAsync(User user, string roleName)
        {
            var roleEntity = await _roleRepository.Table.SingleOrDefaultAsync(r => r.Name.ToUpper() == roleName.ToUpper());
            var ur = new UserRole { UserId = user.Id, RoleId = roleEntity.Id };
            _userRoleRepository.Insert(ur);
        }

        public virtual async Task RemoveFromRoleAsync(User user, string roleName)
        {
            var roleEntity = await _roleRepository.Table.SingleOrDefaultAsync(r => r.Name.ToUpper() == roleName.ToUpper());
            if (roleEntity != null)
            {
                var userRole = await _userRoleRepository.Table.FirstOrDefaultAsync(r => roleEntity.Id.Equals(r.RoleId) && r.UserId.Equals(user.Id));
                if (userRole != null)
                {
                    _userRoleRepository.Delete(userRole);
                }
            }
        }

        public virtual async Task<IList<string>> GetRolesAsync(User user)
        {
            var userId = user.Id;
            var query = from userRole in _userRoleRepository.Table
                        join role in _roleRepository.Table on userRole.RoleId equals role.Id
                        where userRole.UserId.Equals(userId)
                        select role.Name;
            return await query.ToListAsync();
        }

        public virtual async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            var role = await _roleRepository.Table.SingleOrDefaultAsync(r => r.Name.ToUpper() == roleName.ToUpper());
            if (role != null)
            {
                var userId = user.Id;
                var roleId = role.Id;
                return await _userRoleRepository.Table.AnyAsync(ur => ur.RoleId.Equals(roleId) && ur.UserId.Equals(userId));
            }
            return false;
        }

        /// <summary>
        /// 获取失败次数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// 是否开启账号锁
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        ///// <summary>
        ///// 获取账号封锁截止时间
        ///// </summary>
        ///// <param name="user"></param>
        ///// <returns></returns>
        //public virtual Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        //{
        //    return Task.FromResult(user.LockoutEnd);
        //}

        /// <summary>
        /// 增加错误次数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Task<int> IncrementAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount++;
            _userRepository.Update(user);
            return Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// 重置失败次数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Task ResetAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount = 0;
            _userRepository.Update(user);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 设置账号封锁开关
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public virtual Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            _userRepository.Update(user);
            return Task.FromResult(0);
        }

        ///// <summary>
        ///// 设置账号封锁截止时间
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="lockoutEnd"></param>
        ///// <returns></returns>
        //public virtual Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        //{
        //    user.LockoutEnd = lockoutEnd;
        //    _userRepository.Update(user);
        //    return Task.FromResult(0);
        //}

        /// <summary>
        /// 二次验证是否开启
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        /// <summary>
        /// 二次验证开关
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public virtual Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            _userRepository.Update(user);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 添加Claim
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public virtual Task AddClaimAsync(User user, Claim claim)
        {
            var userClaim = new UserClaim { UserId = user.Id, ClaimType = claim.Type, ClaimValue = claim.Value };
            _userClaimRepository.InsertAsync(userClaim);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 获取Claim
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task<IList<Claim>> GetClaimsAsync(User user)
        {
            List<Claim> claims = new List<Claim>();
            await _userClaimRepository.Table.Where(c => c.UserId == user.Id).ForEachAsync(n =>
                claims.Add(new Claim(n.ClaimType, n.ClaimValue)));
            return claims;
        }

        /// <summary>
        /// 移除Claim
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public virtual async Task RemoveClaimAsync(User user, Claim claim)
        {
            var userClaim = await _userClaimRepository.Table.FirstOrDefaultAsync(n => n.UserId == user.Id && n.ClaimType == claim.Type && n.ClaimValue == claim.Value);
            await _userClaimRepository.DeleteAsync(userClaim);
        }

        /// <summary>
        /// 获取可查询的用户集合
        /// </summary>
        public virtual IQueryable<User> Users
        {
            get { return _userRepository.Table; }
        }

        public virtual void Dispose()
        {

        }

        /// <summary>
        /// 获取用户电话号码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPhoneNumberAsync(User user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        /// <summary>
        /// 获取用户电话号码是否验证通过
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetPhoneNumberConfirmedAsync(User user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        /// <summary>
        /// 设置用户电话号码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Task SetPhoneNumberAsync(User user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            _userRepository.UpdateAsync(user);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 设置用户电话号码是否验证通过
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            _userRepository.UpdateAsync(user);
            return Task.FromResult(0);
        }
    }
}
