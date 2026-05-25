using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.ChatMessages.Queries;

public class List
{
    public record Query(string RoomId) : IRequest<Result<List<ChatMessage>>>;

    public class Handler(IChatMessageRepository chatMessageRepository)
        : IRequestHandler<Query, Result<List<ChatMessage>>>
    {
        public async Task<Result<List<ChatMessage>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var chatMessages = await chatMessageRepository.GetAllAsync(
                request.RoomId,
                cancellationToken
            );

            return Result<List<ChatMessage>>.Success(chatMessages);
        }
    }
}
