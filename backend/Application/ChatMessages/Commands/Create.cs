using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.ChatMessages.Commands;

public class Create
{
    public record Command(string UserId, string RoomId, string Content)
        : IRequest<Result<ChatMessage>>;

    public class Handler(
        IChatMessageRepository chatMessageRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork
    ) : IRequestHandler<Command, Result<ChatMessage>>
    {
        public async Task<Result<ChatMessage>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                return Result<ChatMessage>.Failure("User does not exist");

            var message = new ChatMessage(request.RoomId, request.UserId, request.Content);
            chatMessageRepository.Add(message);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            message.Sender = user;

            return Result<ChatMessage>.Success(message);
        }
    }
}
