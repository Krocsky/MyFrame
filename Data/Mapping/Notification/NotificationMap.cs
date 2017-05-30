using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 反馈表映射
    /// </summary>
    public class NotificationMap : EntityTypeConfiguration<Notification>
    {
        public NotificationMap()
        {
            this.ToTable("MP_Notification");
            this.HasKey(c => c.Id);
            this.Property(c => c.Title).IsRequired();
            this.Property(c => c.Content).IsRequired();
            this.Property(c => c.CreateUserId).IsRequired();
            this.Property(c => c.CreateTime).IsOptional();
            this.Property(c => c.IsDelete).IsRequired();
            this.Property(c => c.PushStatus).IsRequired();
            this.Property(c => c.PushTime).IsRequired();

            this.HasRequired(c => c.UserTable).WithMany().HasForeignKey(c=>c.CreateUserId).WillCascadeOnDelete(false);
        }
    }
}
