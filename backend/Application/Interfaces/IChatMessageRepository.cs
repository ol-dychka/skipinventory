using Domain;

namespace Application.Interfaces;

public interface IChatMessageRepository
{
    void Add(ChatMessage message);
    Task<List<ChatMessage>> GetAllAsync(string roomId, CancellationToken cancellationToken);
}
