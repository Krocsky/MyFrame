using System;

namespace Common
{
    /// <summary>
    /// 用户角色关联
    /// </summary>
    public partial class UserRole
    {
        /// <summary>
        ///用户Id
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public virtual int RoleId { get; set; }
    }
}
