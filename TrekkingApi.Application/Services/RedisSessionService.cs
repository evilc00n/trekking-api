using StackExchange.Redis;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using TrekkingApi.Domain.Interfaces.Databases;
using TrekkingApi.Domain.Options.JsonSerializeOptions;
using TrekkingApi.Domain.Result;

namespace TrekkingApi.Application.Services
{
    public class RedisSessionService : ISessionService
    {
        private readonly IDatabase _redis;
        private readonly TimeSpan _sessionTtl = TimeSpan.FromDays(30);

        public RedisSessionService(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public async Task<TimeSpan?> GetSessionTtlAsync(string sessionId)
        {
            return await _redis.KeyTimeToLiveAsync($"session:{sessionId}");
        }




        public BaseResult<ClaimsPrincipal> GetPrincipals(string sessionId, string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("SessionId", sessionId)
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);
            return new BaseResult<ClaimsPrincipal>()
            {
                Data = principal
            };
        }


        public async Task<BaseResult<string>> CreateSessionAsync(string username)
        {
            var sessionId = Guid.NewGuid().ToString();
            var sessionData = JsonSerializer.Serialize(new
            {
                Username = username,
                CreatedAt = DateTime.UtcNow
            }, JsonSerializeOptionsSet.mainOptions);

            await _redis.StringSetAsync($"session:{sessionId}", sessionData, _sessionTtl);

            return new BaseResult<string>()
            {
                Data = sessionId
            };
        }




        public async Task<bool> IsSessionValidAsync(string sessionId)
        {
            return await _redis.KeyExistsAsync($"session:{sessionId}");
        }

        public async Task InvalidateSessionAsync(string sessionId)
        {
            await _redis.KeyDeleteAsync($"session:{sessionId}");
        }


        public Task RefreshSessionAsync(string sessionId)
        {
            return _redis.KeyExpireAsync($"session:{sessionId}", _sessionTtl);
        }


    }
}
