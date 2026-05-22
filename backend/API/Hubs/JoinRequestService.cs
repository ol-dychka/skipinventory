using API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class JoinRequestService(IHubContext<NotificationsHub> hub)
{
    private readonly IHubContext<NotificationsHub> _hub = hub;

    public async Task ApproveRequest(string userId)
    {
        await _hub.Clients.User(userId).SendAsync("JoinRequestApproved");
    }
}
