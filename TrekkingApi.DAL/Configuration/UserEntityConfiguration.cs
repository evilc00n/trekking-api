
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrekkingApi.Domain.Entity;

namespace TrekkingApi.DAL.Configuration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {

            builder.ToTable("users");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(e => e.Login).HasColumnName("login").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()").IsRequired();

            builder.HasIndex(e => e.Login).IsUnique();

        }
    }
}
