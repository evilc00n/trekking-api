
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrekkingApi.Domain.Entity;

namespace TrekkingApi.DAL.Configuration
{
    public class PinMetaEntityConfiguration : IEntityTypeConfiguration<PinMetaEntity>
    {
        public void Configure(EntityTypeBuilder<PinMetaEntity> builder)
        {
            builder.ToTable("pins_meta");

            builder.HasKey(e => e.PinId);
            builder.Property(e => e.PinId).HasColumnName("pin_id");
            builder.Property(e => e.Size).HasColumnName("size").IsRequired();
            builder.Property(e => e.Height).HasColumnName("height").IsRequired();
            builder.Property(e => e.Width).HasColumnName("width").IsRequired();
            builder.Property(e => e.LocationId).HasColumnName("location_id");

            builder.HasOne(e => e.Pin)
                   .WithOne(p => p.Meta)
                   .HasForeignKey<PinMetaEntity>(e => e.PinId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Location)
                   .WithMany(l => l.PinMetas)
                   .HasForeignKey(e => e.LocationId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
