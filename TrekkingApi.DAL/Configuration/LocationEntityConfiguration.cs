
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TrekkingApi.Domain.Entity;

namespace TrekkingApi.DAL.Configuration
{ 

    public class LocationEntityConfiguration : IEntityTypeConfiguration<LocationEntity>
    {
        public void Configure(EntityTypeBuilder<LocationEntity> builder)
        {
            builder.ToTable("locations");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(e => e.Lat).HasColumnName("lat").IsRequired();
            builder.Property(e => e.Lng).HasColumnName("lng").IsRequired();
            builder.Property(e => e.AddressName).HasColumnName("address_name").IsRequired();
        }
    }
}
