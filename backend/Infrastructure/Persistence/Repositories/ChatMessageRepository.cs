using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ChatMessageRepository(PsqlDbContext context) : IChatMessageRepository
{
    private readonly PsqlDbContext _context = context;

    public void Add(ChatMessage message)
    {
        _context.ChatMessages.Add(message);
    }

    public Task<List<ChatMessage>> GetAll(string roomId, CancellationToken cancellationToken)
    {
        return _context
            .ChatMessages.Where(cm => cm.RoomId == roomId)
            .ToListAsync(cancellationToken);
    }
}
