using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 导航映射
    /// </summary>
    public class NavigationMap : EntityTypeConfiguration<Navigation>
    {
        public NavigationMap()
        {
            this.ToTable("Navigations");
            this.HasKey(c => c.Id);
            this.Property(c => c.ParentId).IsOptional();
            this.Property(c => c.Name).IsRequired().HasMaxLength(64);
            this.Property(c => c.RouteName).HasMaxLength(64);
            this.Property(c => c.Url).HasMaxLength(255);
            this.Property(c => c.PermissionName).HasMaxLength(64);
            this.Property(c => c.IconName).HasMaxLength(64);
            this.Property(c => c.DisplayOrder).IsRequired();
            this.Property(c => c.IsEnabled).IsRequired();

            this.HasMany(c => c.Children).WithOptional().HasForeignKey(n => n.ParentId);
            this.HasOptional(c => c.Parent).WithMany().HasForeignKey(n => n.ParentId);
        }
    }
}
