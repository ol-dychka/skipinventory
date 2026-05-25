using API.DTOs.Responses;
using Application.ChatMessages.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ChatMessageController : BaseAPIController
{
    [HttpGet("{roomId}")]
    public async Task<IActionResult> GetChatMessages(string roomId)
    {
        // room id is just an organization id, because chat-organization is a 1-to-1 relationship
        // topic subdivision or chat creation will be followed by using a more spicified room-id -
        // (e.g. "org_id-main") or chat entity creation on a db level
        if (OrganizationId == null)
            return Unauthorized("token does not exist");

        var result = await Mediator.Send(new List.Query(roomId));
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        var chatMessages = result.Value.Select(cm => new ChatMessageDto(cm));
        return Ok(chatMessages);
    }
}
