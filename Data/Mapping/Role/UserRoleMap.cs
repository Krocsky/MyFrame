using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 用户角色映射
    /// </summary>
    public class UserRoleMap : EntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
        {
            this.ToTable("UserRoles");
            this.HasKey(c => new { c.UserId, c.RoleId });
        }
    }
}
