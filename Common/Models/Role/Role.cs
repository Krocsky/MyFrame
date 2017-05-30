using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;

namespace Common
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public partial class Role : IRole<int>, IEntity
    {
        public Role()
        { }

        public Role(string name)
        {
            this.Name = name;
        }

        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 角色职能
        /// </summary>
        public RoleType Type { get; set; }

        /// <summary>
        /// 是否为内置角色
        /// </summary>
        public bool IsBuiltIn { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 角色下的用户集合
        /// </summary>
        public virtual ICollection<UserRole> Users { get; set; }

        /// <summary>
        /// 角色下的权限集合
        /// </summary>
        public virtual ICollection<RolePermission> Permissions { get; set; }
    }

    /// <summary>
    /// 角色职能
    /// </summary>
    public enum RoleType
    {
        /// <summary>
        /// 客户
        /// </summary>
        [Display(Name = "客户")]
        Customer = 2,

        /// <summary>
        /// 公司内部
        /// </summary>
        [Display(Name = "公司内部")]
        Company = 3,

        /// <summary>
        /// 系统用户
        /// </summary>
        [Display(Name = "系统用户")]
        System = 1,
    }
}
