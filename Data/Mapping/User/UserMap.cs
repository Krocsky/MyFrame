using Common;
using System.Data.Entity.ModelConfiguration;

namespace Data
{
    /// <summary>
    /// 用户映射
    /// </summary>
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("Users");
            this.HasKey(c => c.Id);
            //IsRequired
            this.Property(c => c.UserName).IsRequired().HasMaxLength(64);
            this.Property(c => c.Password).IsRequired().HasMaxLength(64);
            //already set default value => false
            this.Property(c => c.IsDeposit).IsRequired();
            this.Property(c => c.IsBackDeposit).IsRequired();
            this.Property(c => c.IsTrueName).IsRequired();
            this.Property(c => c.IsBlackUser).IsRequired();

            this.Property(c => c.UserType).IsRequired();
            this.Property(c => c.TrueName).IsRequired().HasMaxLength(64);
            this.Property(c => c.Password).IsRequired().HasMaxLength(128);
            this.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(64);

            this.Property(c => c.IsDelete).IsRequired();
            this.Property(c => c.CreateTime).IsRequired();

            //IsOptional
            this.Property(c => c.Gender).IsOptional();
            this.Property(c => c.NickName).IsOptional();
            this.Property(c => c.AvatarId).IsOptional();
            this.Property(c => c.IdNumber).IsOptional();
            this.Property(c => c.DeviceName).IsOptional();
            this.Property(c => c.DeviceCode).IsOptional();
            this.Property(c => c.AllUseTime).IsOptional();
            this.Property(c => c.AllUseTimes).IsOptional();
            this.Property(c => c.TotalAmount).IsOptional();
            this.Property(c => c.IDCardPositiveId).IsOptional();
            this.Property(c => c.IDCardOtherId).IsOptional();
            this.Property(c => c.Email).IsOptional().HasMaxLength(64);
            this.Property(c => c.EmailConfirmed).IsOptional();
            this.Property(c => c.Description).IsOptional();
            this.Property(c => c.VerifyCode).IsOptional();
            this.Property(c => c.CurrentAmount).IsOptional();
            this.Property(c => c.DepositMoney).IsOptional();

            //Province.City.Area
            this.Property(c => c.ProvinceId).IsOptional();
            this.Property(c => c.CityId).IsOptional();
            this.Property(c => c.AreaId).IsOptional();

            //RelationShip
            this.HasMany(c => c.Roles).WithOptional().HasForeignKey(n => n.UserId);
            this.HasMany(c => c.Permissions).WithOptional().HasForeignKey(n => n.UserId);

            this.HasOptional(c => c.ProvinceTable).WithMany().HasForeignKey(n=>n.ProvinceId).WillCascadeOnDelete(false);
            this.HasOptional(c => c.CityTable).WithMany().HasForeignKey(n => n.CityId).WillCascadeOnDelete(false);
            this.HasOptional(c => c.AreaTable).WithMany().HasForeignKey(n => n.AreaId).WillCascadeOnDelete(false);
            this.HasOptional(c => c.AvatarTable).WithMany().HasForeignKey(n => n.AvatarId).WillCascadeOnDelete(false);
        }
    }
}
