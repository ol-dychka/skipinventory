using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string roomId, string content)
    {
        var message = new
        {
            sender = Context.UserIdentifier,
            content,
            timestamp = DateTime.UtcNow,
        };
        await Clients.Group(roomId).SendAsync("ReceiveMessage", message);
    }

    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("UserJoined", Context.UserIdentifier);
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("UserLeft", Context.UserIdentifier);
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        // track which rooms user is in (cache), clean those up
        await base.OnDisconnectedAsync(ex);
    }
}
