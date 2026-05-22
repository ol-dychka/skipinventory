namespace Domain;

public class ChatMessage(string roomId, string senderId, string content)
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string RoomId { get; set; } = roomId;
    public string SenderId { get; set; } = senderId;
    public string Content { get; set; } = content;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
