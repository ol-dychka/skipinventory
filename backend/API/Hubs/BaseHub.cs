using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

[Authorize]
public class BaseHub : Hub
{
    private IMediator? _mediator;
    protected IMediator Mediator =>
        _mediator ??=
            Context.GetHttpContext()!.RequestServices.GetService<IMediator>()
            ?? throw new InvalidOperationException("IMediator service is not available");

    protected string UserId => Context.UserIdentifier!; //always there with [Authorize] flag
    protected string OrganizationId => Context.User!.FindFirst("org_id")?.Value;
}
