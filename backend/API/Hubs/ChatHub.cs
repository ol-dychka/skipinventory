using API.DTOs.Responses;
using Application.ChatMessages.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class ChatHub : BaseHub
{
    // room id is just an organization id, because chat-organization is a 1-to-1 relationship

    // in the future, topic subdivision or chat creation will be followed by using a more specified
    // room-id - (e.g. "org_id-main") or chat entity creation on a db level
    public async Task SendMessage(string content)
    {
        if (OrganizationId == null || UserId == null)
        {
            Console.WriteLine("NOT AUTHENTICATED");
            Console.WriteLine($"User ID: {UserId}");

            Console.WriteLine($"Org ID: {OrganizationId}");

            await Clients.Caller.SendAsync("Error", "token doesn't have organization info");
            return;
        }

        var result = await Mediator.Send(new Create.Command(UserId, OrganizationId, content));
        if (!result.IsSuccess || result.Value == null)
        {
            await Clients.Caller.SendAsync("Error", result.Error);
            return;
        }

        var message = new ChatMessageDto(result.Value);

        await Clients.Group(OrganizationId).SendAsync("ReceiveMessage", message);
    }

    public async Task JoinRoom()
    {
        if (Context.User?.Identity?.IsAuthenticated != true)
        {
            await Clients.Caller.SendAsync("Error", "User is not authenticated");
            return;
        }

        if (OrganizationId == null)
        {
            await Clients.Caller.SendAsync("Error", "token doesn't have organization info");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, OrganizationId);
        await Clients.Group(OrganizationId).SendAsync("UserJoined", UserId);
    }

    public async Task LeaveRoom()
    {
        if (OrganizationId == null)
        {
            await Clients.Caller.SendAsync("Error", "token doesn't have organization info");
            return;
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, OrganizationId);
        await Clients.Group(OrganizationId).SendAsync("UserLeft", UserId);
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
