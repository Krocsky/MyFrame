using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 设备表映射
    /// </summary>
    public class SettingMap : EntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            this.ToTable("MP_Settings");
            this.HasKey(c => c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(64);
            this.Property(c => c.Number).IsOptional();
            this.Property(c => c.Code).IsOptional();
            this.Property(c => c.IsDelete).IsRequired();
            this.Property(c => c.CreatedTime).IsRequired();
        }
    }
}
