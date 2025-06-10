
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrekkingApi.Domain.Entity;

namespace TrekkingApi.DAL.Configuration
{
    public class CollectionPinsEntityConfiguration : IEntityTypeConfiguration<CollectionsPinsEntity>
    {
        public void Configure(EntityTypeBuilder<CollectionsPinsEntity> builder)
        {
            builder.ToTable("collections_pins");

            builder.HasKey(e => new { e.PinId, e.CollectionId });
            builder.Property(e => e.PinId).HasColumnName("pin_id");
            builder.Property(e => e.CollectionId).HasColumnName("collection_id");

            builder.HasOne(e => e.Pin)
                   .WithMany(p => p.CollectionsPins)
                   .HasForeignKey(e => e.PinId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Collection)
                   .WithMany(c => c.CollectionsPins)
                   .HasForeignKey(e => e.CollectionId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
