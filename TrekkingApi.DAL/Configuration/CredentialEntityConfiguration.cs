
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TrekkingApi.Domain.Entity;
using System.Net;



namespace TrekkingApi.DAL.Configuration
{
    public class CredentialEntityConfiguration : IEntityTypeConfiguration<CredentialEntity>
    {
        public void Configure(EntityTypeBuilder<CredentialEntity> builder)
        {
            builder.ToTable("credentials");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(e => e.Hash).HasColumnName("hash").IsRequired();
            builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();

            builder.HasOne(e => e.User)
                   .WithOne(u => u.Credential)
                   .HasForeignKey<CredentialEntity>(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.UserId).IsUnique(); // Для связи один-к-одному
        }
    }
}
