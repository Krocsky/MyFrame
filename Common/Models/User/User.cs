using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;

namespace Common
{
    /// <summary>
    /// 用户
    /// </summary>
    public partial class User : IUser<int>, IEntity
    {
        public User()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">登录账号</param>
        /// <param name="nickName">用户姓名</param>
        /// <param name=""></param>
        public User(string userName, string nickName)
        {
            this.UserName = userName;
            this.TrueName = nickName;
            this.IsDelete = false;
            this.CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 生成应用本地Identity验证缓存
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        /// <summary>
        /// 生成应用本地Identity验证类型
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsyncAuthenticationType(UserManager<User, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, "Cookies");
            return userIdentity;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderType? Gender { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像Id
        /// </summary>
        public int? AvatarId { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public int? ProvinceId { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public int? AreaId { get; set; }

        /// <summary>
        /// 是否验证
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// 是否缴纳押金
        /// </summary>
        [DefaultValue(false)]
        public bool IsDeposit { get; set; }

        /// <summary>
        /// 押金金额
        /// </summary>
        public double DepositMoney { get; set; }

        /// <summary>
        /// 是否退回押金
        /// </summary>
        [DefaultValue(false)]
        public bool IsBackDeposit { get; set; }

        /// <summary>
        /// 是否实名认证
        /// </summary>
        [DefaultValue(false)]
        public bool IsTrueName { get; set; }

        /// <summary>
        /// 身份证正面Id
        /// </summary>
        public int IDCardPositiveId { get; set; }

        /// <summary>
        /// 身份证反面Id
        /// </summary>
        public int IDCardOtherId { get; set; }

        /// <summary>
        /// 当前余额
        /// </summary>
        public double CurrentAmount { get; set; }

        /// <summary>
        /// 用户设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 用户设备唯一标识符
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 用户总使用时长
        /// </summary>
        public decimal AllUseTime { get; set; }

        /// <summary>
        /// 用户使用次数
        /// </summary>
        public int AllUseTimes { get; set; }

        /// <summary>
        /// 消费总金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 黑名单标识
        /// </summary>
        [DefaultValue(false)]
        public bool IsBlackUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 邮箱确认
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 密码输入错误次数
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// 账号是否可以被封锁
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// 用户注释
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 二次验证开关
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 用户的角色集合
        /// </summary>
        public virtual ICollection<UserRole> Roles { get; set; }

        /// <summary>
        /// 用户的权限集合
        /// </summary>
        public virtual ICollection<UserPermission> Permissions { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public virtual ChinaArea ProvinceTable { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public virtual ChinaArea CityTable { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public virtual ChinaArea AreaTable { get; set; }

        /// <summary>
        /// 头像表
        /// </summary>
        public virtual Attachment AvatarTable { get; set; }
    }
}
