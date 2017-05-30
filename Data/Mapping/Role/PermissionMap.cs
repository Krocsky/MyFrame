using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 权限映射
    /// </summary>
    public class PermissionMap : EntityTypeConfiguration<Permission>
    {
        public PermissionMap()
        {
            this.ToTable("Permissions");
            this.HasKey(c => c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(64);
            this.Property(c => c.Description).HasMaxLength(64);
            this.Property(c => c.ParentId).IsOptional();
            this.Property(c => c.IsBuiltIn).IsRequired();
            this.Property(c => c.IsButton).IsRequired();
            this.Property(c => c.RolePermissonEnumType).IsRequired();

            this.HasMany(c => c.Roles).WithOptional().HasForeignKey(n => n.PermissionId);
        }
    }
}
