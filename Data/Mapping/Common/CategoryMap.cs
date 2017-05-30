using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 分类映射
    /// </summary>
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            this.ToTable("Categories");
            this.HasKey(c => c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(64);
            this.Property(c => c.Description);
            this.Property(c => c.ParentId).IsOptional();
            this.Property(c => c.SchoolId).IsRequired();

            this.HasMany(c => c.Children).WithOptional().HasForeignKey(n => n.ParentId);
            this.HasOptional(c => c.Parent).WithMany().HasForeignKey(n => n.ParentId);
        }
    }
}
