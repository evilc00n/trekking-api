using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using TrekkingApi.Domain.Interfaces.Databases;

namespace TrekkingApi.Api.Handlers
{
    public class RedisSessionValidator : AuthorizationHandler<RedisSessionRequirement>
    {
        private readonly ISessionService _sessionService;

        public RedisSessionValidator(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RedisSessionRequirement requirement)
        {
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;  
        }
    }

    public class RedisSessionRequirement : IAuthorizationRequirement { }
}
