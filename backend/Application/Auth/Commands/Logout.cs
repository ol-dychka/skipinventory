using System;
using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Auth.Commands;

public class Logout
{
    public record Response(string UserId);

    public record Command(string RefreshToken) : IRequest<Result<Response>>;

    public class Handler(
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork,
        ITokenGenerator tokenGenerator
    ) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var refreshTokenHash = tokenGenerator.HashRefreshToken(request.RefreshToken);

            var refreshToken = await refreshTokenRepository.GetByHashAsync(
                refreshTokenHash,
                cancellationToken
            );

            if (
                refreshToken == null
                || refreshToken.IsRevoked
                || refreshToken.ExpiresAt < DateTime.UtcNow
            )
                return Result<Response>.Failure("Session is invalid or has expired");

            refreshToken.IsRevoked = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Response>.Success(new Response(refreshToken.UserId));
        }
    }
}
