using System;

namespace Common
{
    /// <summary>
    /// 用户Claim
    /// </summary>
    public partial class UserClaim : IEntity
    {
        public virtual int Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// Claim类型
        /// </summary>
        public virtual string ClaimType { get; set; }

        /// <summary>
        /// Claim值
        /// </summary>
        public virtual string ClaimValue { get; set; }
    }
}
