using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 自运行任务映射
    /// </summary>
    public class QuartzTaskMap : EntityTypeConfiguration<QuartzTask>
    {
        public QuartzTaskMap()
        {
            this.ToTable("MP_QuartzTasks");
            this.HasKey(c => c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(128);
            this.Property(c => c.ClassType).HasMaxLength(128);
            this.Property(c => c.TaskRule).HasMaxLength(64);
            this.Property(c => c.Enabled).IsRequired();
            this.Property(c => c.IsRunning).IsRequired();
            this.Property(c => c.StartDate).IsRequired();
            this.Property(c => c.EndDate).IsOptional();
            this.Property(c => c.LastStart).IsOptional();
            this.Property(c => c.LastEnd).IsOptional();
            this.Property(c => c.LastIsSuccess).IsOptional();
            this.Property(c => c.LastFailMessage).IsOptional();
            this.Property(c => c.NextStart).IsOptional();
        }
    }
}
