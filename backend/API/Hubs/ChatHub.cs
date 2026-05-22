using Application.ChatMessages.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class ChatHub : BaseHub
{
    public async Task SendMessage(string roomId, string content)
    {
        var result = await Mediator.Send(new Create.Command(UserId, roomId, content));
        if (!result.IsSuccess)
        {
            await Clients.Caller.SendAsync("Error", result.Error);
            return;
        }

        await Clients.Group(roomId).SendAsync("ReceiveMessage", result.Value);
    }

    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("UserJoined", UserId);
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("UserLeft", UserId);
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        // if tracking which rooms user is in (cache), clean them up
        await base.OnDisconnectedAsync(ex);
    }
}
