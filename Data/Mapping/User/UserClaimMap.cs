using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 用户Claim映射
    /// </summary>
    public class UserClaimMap : EntityTypeConfiguration<UserClaim>
    {
        public UserClaimMap()
        {
            this.ToTable("UserClaims");
            this.HasKey(uc => uc.Id);
        }
    }
}
