using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 角色映射
    /// </summary>
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            this.ToTable("Roles");
            this.HasKey(c => c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(64);
            this.Property(c => c.Description).HasMaxLength(128);
            this.Property(c => c.IsBuiltIn).IsRequired();
            this.Property(c => c.Type).IsRequired();
            this.Property(c => c.DisplayOrder).IsRequired();

            this.HasMany(c => c.Users).WithOptional().HasForeignKey(n => n.RoleId);
            this.HasMany(c => c.Permissions).WithOptional().HasForeignKey(n => n.RoleId);
        }
    }
}
