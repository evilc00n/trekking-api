
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrekkingApi.Domain.Entity;

namespace TrekkingApi.DAL.Configuration
{
    public class CollectionEntityConfiguration : IEntityTypeConfiguration<CollectionEntity>
    {
        public void Configure(EntityTypeBuilder<CollectionEntity> builder)
        {
            builder.ToTable("collections");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(e => e.Title).HasColumnName("title").IsRequired();
            builder.Property(e => e.OwnerId).HasColumnName("owner_id").IsRequired();
            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()").IsRequired();

            builder.HasOne(e => e.Owner)
                   .WithMany(u => u.Collections)
                   .HasForeignKey(e => e.OwnerId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
