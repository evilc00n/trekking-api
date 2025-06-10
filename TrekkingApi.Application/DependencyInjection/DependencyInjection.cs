using Microsoft.Extensions.DependencyInjection;
using TrekkingApi.Application.Mapping;
using TrekkingApi.Application.Services;
using TrekkingApi.Domain.Interfaces.Databases;
using TrekkingApi.Domain.Interfaces.Services;

namespace TrekkingApi.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddApplications(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserMapping));
            services.InitServices();
        }

        private static void InitServices(this IServiceCollection services)
        {
            services.AddSingleton<ISessionService, RedisSessionService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPinService, PinService>();

        }
    }
}

