
using System.Security.Claims;
using TrekkingApi.Domain.Result;

namespace TrekkingApi.Domain.Interfaces.Databases
{
    public interface ISessionService
    {
        Task<BaseResult<string>> CreateSessionAsync(string username);
        BaseResult<ClaimsPrincipal> GetPrincipals(string sessionId, string username);
        Task<bool> IsSessionValidAsync(string sessionId);
        Task InvalidateSessionAsync(string sessionId);
        Task<TimeSpan?> GetSessionTtlAsync(string sessionId);

        Task RefreshSessionAsync(string sessionId);



    }
}
