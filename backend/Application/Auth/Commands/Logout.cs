using System;
using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Auth.Commands;

public class Logout
{
    public record Command(string RefreshToken) : IRequest<Result<Unit>>;

    public class Handler(
        IRefreshTokenRepository refreshTokenRepository,
        ITokenGenerator tokenGenerator
    ) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var refreshTokenHash = tokenGenerator.HashRefreshToken(request.RefreshToken);

            var refreshToken = await refreshTokenRepository.
                GetByHashAsync(refreshTokenHash, cancellationToken);

            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiresAt < DateTime.UtcNow)
                return Result<Unit>.Failure("Session is invalid or has expired");

            refreshToken.IsRevoked = true;

            await refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value); 
        }
    }
}
