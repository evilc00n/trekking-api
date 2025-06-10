
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using TrekkingApi.DAL.Repositories;
using TrekkingApi.Domain.Entity;
using TrekkingApi.Domain.Interfaces.Databases;
using TrekkingApi.Domain.Interfaces.Repositories;

namespace TrekkingApi.DAL.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var userConnectionString = configuration["USER_DB"];
          

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(userConnectionString);
            var dataSource = dataSourceBuilder.Build();


            services.AddDbContext<DbUsersContext>(options =>
            {
                options.UseNpgsql(dataSource);
            });

            services.InitRepositories();
        }


        private static void InitRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<UserEntity>, BaseRepository<UserEntity>>();
            services.AddScoped<IBaseRepository<CredentialEntity>, BaseRepository<CredentialEntity>>();
            services.AddScoped<IBaseRepository<PinEntity>, BaseRepository<PinEntity>>();
            services.AddScoped<IBaseRepository<PinMetaEntity>, BaseRepository<PinMetaEntity>>();
            services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
            services.AddScoped<IPinUnitOfWork, PinUnitOfWork>();
        }

    }
}

