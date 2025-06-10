
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrekkingApi.Domain.Entity;

namespace TrekkingApi.DAL.Configuration
{
    public class UserSettingsEntityConfiguration : IEntityTypeConfiguration<UserSettingsEntity>
    {
        public void Configure(EntityTypeBuilder<UserSettingsEntity> builder)
        {
            builder.ToTable("users_settings");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();

            builder.HasOne(e => e.User)
                   .WithOne(u => u.Settings)
                   .HasForeignKey<UserSettingsEntity>(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
