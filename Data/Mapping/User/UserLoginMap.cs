using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 用户登陆信息映射
    /// </summary>
    public class UserLoginMap : EntityTypeConfiguration<UserLogin>
    {
        public UserLoginMap()
        {
            this.ToTable("UserLogins");
            this.HasKey(l => new { l.LoginProvider, l.ProviderKey });
        }
    }
}
