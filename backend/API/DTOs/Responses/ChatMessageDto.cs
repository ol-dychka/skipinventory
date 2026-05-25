using Domain;

namespace API.DTOs.Responses;

public class ChatMessageDto(ChatMessage cm)
{
    public string Id { get; set; } = cm.Id;
    public string RoomId { get; set; } = cm.RoomId;
    public string SenderId { get; set; } = cm.SenderId;
    public string Content { get; set; } = cm.Content;
    public DateTime SentAt { get; set; } = cm.SentAt;
    public string SenderName { get; set; } = cm.Sender.Name;
}
