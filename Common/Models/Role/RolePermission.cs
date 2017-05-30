using System;
using System.ComponentModel.DataAnnotations;

namespace Common
{
    /// <summary>
    /// 角色权限关联
    /// </summary>
    public partial class RolePermission
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public virtual int RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        public virtual int PermissionId { get; set; }

        /// <summary>
        /// 角色权限类型
        /// </summary>
        public RolePermissonEnum RolePermissonEnumType { get; set; }

        ///// <summary>
        ///// 是否为button
        ///// </summary>
        //public bool IsButton { get; set; }
    }

}
