using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 角色权限映射
    /// </summary>
    public class RolePermissionMap : EntityTypeConfiguration<RolePermission>
    {
        public RolePermissionMap()
        {
            this.ToTable("RolePermissions");
            this.HasKey(c => new { c.RoleId, c.PermissionId });
            this.Property(n => n.RolePermissonEnumType).IsRequired();
        }
    }
}
