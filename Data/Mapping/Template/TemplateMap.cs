using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 职位映射
    /// </summary>
    public class TemplateMap : EntityTypeConfiguration<Template>
    {
        public TemplateMap()
        {
            this.ToTable("Templates");
            this.HasKey(c => c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(64);
            this.Property(c => c.Subject).HasMaxLength(128); ;
            this.Property(c => c.Body).IsRequired();
            this.Property(c => c.Type).IsRequired();
        }
    }
}
