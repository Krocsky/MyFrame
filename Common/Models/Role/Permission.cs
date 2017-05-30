using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public partial class Permission : IEntity
    {
        public Permission()
        {}

        public Permission(string name)
        {
            this.Name = name;
        }

        public int Id { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public int?  ParentId { get; set;}

        /// <summary>
        /// 权限描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否为内置权限
        /// </summary>
        public bool IsBuiltIn { get; set; }

        /// <summary>
        /// 是否为按钮权限
        /// </summary>
        public bool IsButton { get; set; }

        /// <summary>
        /// 角色权限类型
        /// </summary>
        public RolePermissonEnum RolePermissonEnumType { get; set; }

        /// <summary>
        /// 权限的角色集合
        /// </summary>
        public virtual ICollection<RolePermission> Roles { get; set; }
    }
    public enum RolePermissonEnum
    {
        [Display(Name = "页签")]
        Tabs = 1,
        [Display(Name = "菜单节点")]
        Navigation = 2
    }
}
