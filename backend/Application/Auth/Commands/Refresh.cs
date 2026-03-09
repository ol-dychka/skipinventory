using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Auth.Commands;

public class Refresh
{
    public record Response(string AccessToken, string RefreshToken);
    public record Command(string RefreshToken) : IRequest<Result<Response>>;

    public class Handler(
        IRefreshTokenRepository refreshTokenRepository,
        ITokenGenerator tokenGenerator
    ) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var currentRefreshTokenHash = tokenGenerator.HashRefreshToken(request.RefreshToken);

            var currentRefreshToken = await refreshTokenRepository.
                GetByHashWithUserAsync(currentRefreshTokenHash, cancellationToken);

            if (currentRefreshToken == null || currentRefreshToken.IsRevoked || currentRefreshToken.ExpiresAt < DateTime.UtcNow)
                return Result<Response>.Failure("Session is invalid or has expired");

            if (currentRefreshToken.User == null)
                return Result<Response>.Failure("Error finding the session user");

            var newRefreshTokenData = tokenGenerator.GenerateRefreshToken();
            var newRefreshTokenHash = tokenGenerator.HashRefreshToken(newRefreshTokenData.Token);
            var newRefreshToken = new RefreshToken(currentRefreshToken.User.Id, newRefreshTokenHash, newRefreshTokenData.ExpiresAt);
            refreshTokenRepository.Add(newRefreshToken);
            
            currentRefreshToken.IsRevoked = true;

            await refreshTokenRepository.SaveChangesAsync(cancellationToken);

            var accessToken = tokenGenerator.
                GenerateAccessToken(currentRefreshToken.User.Id, currentRefreshToken.User.Email);

            return Result<Response>.Success(new Response(accessToken, newRefreshTokenData.Token)); 
        }
    }
}
