using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public partial class UserCreateModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Display(Name = "用户姓名")]
        public string UserName { get; set; }

        /// <summary>
        /// 登录账户
        /// </summary>
        [Display(Name = "登录账户")]
        public string LoginAccount { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Display(Name = "登录密码")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "密码最少6位")]
        public string PasswordModify { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 用户注释
        /// </summary>
        [Display(Name = "用户注释")]
        public string Description { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        [Display(Name = "用户类型")]
        public UserType UserType { get; set; }

        /// <summary>
        /// 省份Code
        /// </summary>
        public string ProvinceCode { get; set; }

        /// <summary>
        /// 城市Code
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        /// 地区Code
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 省份Name
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 城市Name
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 地区Name
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [Display(Name = "角色")]
        public ICollection<Role> Roles { get; set; }
    }
}
