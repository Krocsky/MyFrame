using System;
using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public partial class UserEditModel
    {
        public UserEditModel()
        {
            this.RoleList = new List<RoleEntity>();
        }

        public int Id { get; set; }

        /// <summary>
        /// 登录账户
        /// </summary>
        [Display(Name = "登录账户")]
        public string LoginAccount { get; set; }

        /// <summary>
        /// 所属校区
        /// </summary>
        [Display(Name = "所属校区")]
        public int SchoolAreaId { get; set; }

        /// <summary>
        /// 用户注释
        /// </summary>
        [Display(Name = "用户注释")]
        public string Description { get; set; }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public bool IsSuperAdministrator { get; set; }




        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "昵称")]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像")]
        public string UserHead { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        public Gender Gender { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [Display(Name = "角色")]
        public ICollection<UserRole> Roles { get; set; }

        /// <summary>
        ///  登录名
        /// </summary>
        [Display(Name = "登录名")]
        public string UserName { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        [Display(Name = "名字")]
        public string TrueName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        public string Area { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        public string Password { get; set; }

        /// <summary>
        /// 代理类别
        /// </summary>
        [Display(Name = "代理类别")]
        public int AgentType { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleIds { get; set; }

        public List<RoleEntity> RoleList { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public class RoleEntity
        {
            /// <summary>
            /// 角色名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 角色Id
            /// </summary>
            public int Id { get; set; }
        }

    }
}
