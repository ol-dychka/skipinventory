using System.IdentityModel.Tokens.Jwt;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class RedisTokenValidationFilter(IRedisTokenService redisTokenService) : IAsyncActionFilter
{
    private readonly IRedisTokenService _redisTokenService = redisTokenService;

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var isAllowAnonymous = context
            .ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>()
            .Any();

        if (isAllowAnonymous)
        {
            await next();
            return;
        }

        var userId = context.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        var incomingToken = await context.HttpContext.GetTokenAsync("access_token");

        if (userId == null || incomingToken == null)
        {
            context.Result = new UnauthorizedObjectResult("token does not exist");
            return;
        }

        var isValid = await _redisTokenService.VerifyTokenAsync(userId, incomingToken);
        if (!isValid)
        {
            context.Result = new UnauthorizedObjectResult("token is invalid");
            return;
        }

        await next();
    }
}
