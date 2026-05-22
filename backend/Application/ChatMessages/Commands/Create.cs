using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.ChatMessages.Commands;

public class Create
{
    public record Command(string UserId, string RoomId, string Content)
        : IRequest<Result<ChatMessage>>;

    public class Handler(IChatMessageRepository repository, IUnitOfWork unitOfWork)
        : IRequestHandler<Command, Result<ChatMessage>>
    {
        public async Task<Result<ChatMessage>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var message = new ChatMessage(request.RoomId, request.UserId, request.Content);

            repository.Add(message);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<ChatMessage>.Success(message);
        }
    }
}
