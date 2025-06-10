
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrekkingApi.Domain.Entity;

namespace TrekkingApi.DAL.Configuration
{
    public class PinEntityConfiguration : IEntityTypeConfiguration<PinEntity>
    {
        public void Configure(EntityTypeBuilder<PinEntity> builder)
        {
            builder.ToTable("pins");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(e => e.OwnerId).HasColumnName("owner_id");
            builder.Property(e => e.Title).HasColumnName("title").IsRequired();
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.ThumbnailUrl).HasColumnName("thumbnail_url").IsRequired();
            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()").IsRequired();
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.Category).HasColumnName("category").IsRequired();
            builder.Property(e => e.FullUrl).HasColumnName("full_url").IsRequired();

            builder.HasIndex(e => e.ThumbnailUrl).IsUnique();
            builder.HasIndex(e => e.FullUrl).IsUnique();

            builder.HasOne(e => e.Owner)
                   .WithMany(u => u.Pins)
                   .HasForeignKey(e => e.OwnerId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
