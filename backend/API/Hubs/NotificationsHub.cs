using API.DTOs.Responses;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class NotificationsHub : BaseHub
{
    public async Task JoinRoom()
    {
        if (Context.User?.Identity?.IsAuthenticated != true)
        {
            await Clients.Caller.SendAsync(
                "ReceiveNotification",
                new NotificationDto(NotificationType.error, "Not Authenticated", true)
            );
            return;
        }

        if (UserId == null)
        {
            await Clients.Caller.SendAsync(
                "ReceiveNotification",
                new NotificationDto(NotificationType.error, "Not Authenticated", true)
            );
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, UserId);
        await Clients
            .Group(UserId)
            .SendAsync(
                "ReceiveNotification",
                new NotificationDto(NotificationType.info, "Connected Successfully", true)
            );
    }

    public async Task LeaveRoom()
    {
        if (UserId == null)
        {
            await Clients.Caller.SendAsync(
                "ReceiveNotification",
                new NotificationDto(NotificationType.error, "Not Authenticated", true)
            );
            return;
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, UserId);
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        // if tracking which rooms user is in (cache), clean them up
        await base.OnDisconnectedAsync(ex);
    }

    public override async Task OnConnectedAsync()
    {
        var claims =
            Context.User?.Claims.Select(c => $"{c.Type}: {c.Value}") ?? ["No claims / null user"];

        Console.WriteLine($"Connected user claims: {string.Join(", ", claims)}");

        await base.OnConnectedAsync();
    }
}
