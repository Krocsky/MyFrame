using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 省市区表映射
    /// </summary>
    public class ChinaAreaMap : EntityTypeConfiguration<ChinaArea>
    {
        public ChinaAreaMap()
        {
            this.ToTable("MP_Area");
            this.HasKey(c => c.Id);
            this.Property(c => c.AreaType).IsRequired();
            this.Property(c => c.Name).IsRequired().HasMaxLength(64);
            this.Property(c => c.Code).IsRequired();
            this.Property(c => c.ParentCode).IsOptional();
        }
    }
}
