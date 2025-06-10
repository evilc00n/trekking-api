
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrekkingApi.Domain.Entity;

namespace TrekkingApi.DAL.Configuration
{
    public class FollowEntityConfiguration : IEntityTypeConfiguration<FollowEntity>
    {
        public void Configure(EntityTypeBuilder<FollowEntity> builder)
        {
            builder.ToTable("follows");

            builder.HasKey(e => new { e.FollowerId, e.FollowingId });
            builder.Property(e => e.FollowerId).HasColumnName("follower_id");
            builder.Property(e => e.FollowingId).HasColumnName("following_id");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()").IsRequired();

            builder.HasOne(e => e.Follower)
                   .WithMany(u => u.Following)
                   .HasForeignKey(e => e.FollowerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Following)
                   .WithMany(u => u.Followers)
                   .HasForeignKey(e => e.FollowingId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
