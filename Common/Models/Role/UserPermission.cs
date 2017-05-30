using System;

namespace Common
{
    /// <summary>
    /// 用户权限
    /// </summary>
    public partial class UserPermission
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        public virtual int PermissionId { get; set; }
    }
}
