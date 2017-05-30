using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 用户权限映射
    /// </summary>
    public class UserPermissionMap : EntityTypeConfiguration<UserPermission>
    {
        public UserPermissionMap()
        {
            this.ToTable("UserPermissions");
            this.HasKey(c => new { c.UserId, c.PermissionId });
        }
    }
}
