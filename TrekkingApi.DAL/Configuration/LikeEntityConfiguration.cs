
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TrekkingApi.Domain.Entity;

namespace TrekkingApi.DAL.Configuration
{
    public class LikeEntityConfiguration : IEntityTypeConfiguration<LikeEntity>
    {
        public void Configure(EntityTypeBuilder<LikeEntity> builder)
        {
            builder.ToTable("likes");

            builder.HasKey(e => new { e.EntityId, e.Type });
            builder.Property(e => e.EntityId).HasColumnName("entity_id").IsRequired();
            builder.Property(e => e.Type).HasColumnName("type").IsRequired();

            builder.HasIndex(e => e.EntityId).IsUnique();
        }
    }
}
