using System;

namespace Common
{
    /// <summary>
    /// 用户登录信息
    /// </summary>
    public partial class UserLogin
    {
        /// <summary>
        /// 验证信息
        /// </summary>
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// 验证信息
        /// </summary>
        public virtual string ProviderKey { get; set; }
        
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual int UserId { get; set; }

    }
}
