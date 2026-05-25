using Domain;

namespace API.DTOs.Responses;

public class ChatMessageDto(ChatMessage cm, string currentUserId)
{
    public string Id { get; set; } = cm.Id;
    public string RoomId { get; set; } = cm.RoomId;
    public string SenderId { get; set; } = cm.SenderId;
    public string Content { get; set; } = cm.Content;
    public DateTime SentAt { get; set; } = cm.SentAt;
    public bool IsMine { get; set; } = cm.SenderId == currentUserId;
}
