
using Microsoft.EntityFrameworkCore;
using TrekkingApi.DAL.Configuration;
using TrekkingApi.Domain.Entity;

namespace TrekkingApi.DAL
{
    public class DbUsersContext : DbContext
    {
        public DbUsersContext(DbContextOptions<DbUsersContext> options) : base(options)
        {
        }
        public DbSet<CollectionEntity> Collections { get; set; }
        public DbSet<CollectionsPinsEntity> CollectionsPins { get; set; }
        public DbSet<CredentialEntity> Credentials { get; set; }

        public DbSet<FollowEntity> Follows { get; set; }

        public DbSet<LikeEntity> Likes { get; set; }

        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<PinEntity> Pins { get; set; }
        public DbSet<PinMetaEntity> PinsMeta { get; set; }
        public DbSet<PinViewEntity> PinsViews { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<UserSettingsEntity> UserSettings { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new CollectionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CollectionPinsEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CredentialEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FollowEntityConfiguration());
            modelBuilder.ApplyConfiguration(new LikeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new LocationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PinEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PinMetaEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PinViewEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserSettingsEntityConfiguration());
        }

    }
}
