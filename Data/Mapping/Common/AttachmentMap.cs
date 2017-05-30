using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 附件映射
    /// </summary>
    public class AttachmentMap : EntityTypeConfiguration<Attachment>
    {
        public AttachmentMap()
        {
            this.ToTable("Attachments");
            this.HasKey(c => c.Id);
            this.Property(c => c.FileName).IsRequired().HasMaxLength(255);
            this.Property(c => c.OriginalName).IsRequired().HasMaxLength(255);
            this.Property(c => c.TenantType).IsRequired();
            this.Property(c => c.ItemId).IsRequired();
            this.Property(c => c.CreatedTime).IsRequired();
            this.Property(c => c.UserId).IsRequired();
            this.Property(c => c.FilePath).IsRequired();

            this.HasRequired(c => c.User).WithMany().HasForeignKey(n => n.UserId);
        }
    }
}
