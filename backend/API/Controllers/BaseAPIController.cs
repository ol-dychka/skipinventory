using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseAPIController : ControllerBase
    {
        private IMediator? _mediator;

        protected IMediator Mediator =>
            _mediator ??=
                HttpContext.RequestServices.GetService<IMediator>()
                ?? throw new InvalidOperationException("IMediator service is not available");

        protected string? UserId => User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        protected string? Role => User.FindFirstValue(ClaimTypes.Role);
        protected string? RefreshToken => Request.Cookies["refreshToken"];
        protected string? OrganizationId => User.FindFirst("org_id")?.Value;
    }
}
