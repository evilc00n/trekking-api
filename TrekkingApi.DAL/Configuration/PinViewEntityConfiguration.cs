
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrekkingApi.Domain.Entity;

namespace TrekkingApi.DAL.Configuration
{
    public class PinViewEntityConfiguration : IEntityTypeConfiguration<PinViewEntity>
    {
        public void Configure(EntityTypeBuilder<PinViewEntity> builder)
        {
            builder.ToTable("pins_views");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(e => e.PinId).HasColumnName("pin_id").IsRequired();
            builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();

            builder.HasOne(e => e.Pin)
                   .WithMany(p => p.Views)
                   .HasForeignKey(e => e.PinId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.User)
                   .WithMany(u => u.Views)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
