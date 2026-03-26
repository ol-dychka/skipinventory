using System;
using Application.Core;
using Application.Interfaces;
using Application.Models;
using Domain;
using MediatR;

namespace Application.Auth.Commands;

public class Refresh
{
    public record Response(string AccessToken, RefreshTokenData RefreshTokenData);

    public record Command(string RefreshToken, string? OrganizationId) : IRequest<Result<Response>>;

    public class Handler(
        IRefreshTokenRepository refreshTokenRepository,
        IMemberRepository memberRepository,
        ITokenGenerator tokenGenerator
    ) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var currentRefreshTokenHash = tokenGenerator.HashRefreshToken(request.RefreshToken);

            var currentRefreshToken = await refreshTokenRepository.GetByHashWithUserAsync(
                currentRefreshTokenHash,
                cancellationToken
            );

            if (
                currentRefreshToken == null
                || currentRefreshToken.IsRevoked
                || currentRefreshToken.ExpiresAt < DateTime.UtcNow
            )
                return Result<Response>.Failure("Session is invalid or has expired");

            if (currentRefreshToken.User == null)
                return Result<Response>.Failure("Error finding the session user");

            var newRefreshTokenData = tokenGenerator.GenerateRefreshToken();
            var newRefreshTokenHash = tokenGenerator.HashRefreshToken(newRefreshTokenData.Token);
            var newRefreshToken = new RefreshToken(
                currentRefreshToken.User.Id,
                newRefreshTokenHash,
                newRefreshTokenData.ExpiresAt
            );
            refreshTokenRepository.Add(newRefreshToken);

            currentRefreshToken.IsRevoked = true;
            await refreshTokenRepository.SaveChangesAsync(cancellationToken);

            string accessToken;
            if (request.OrganizationId != null)
            {
                var membership = await memberRepository.GetByOrgIdAsync(
                    request.OrganizationId,
                    cancellationToken
                );
                if (membership == null)
                    return Result<Response>.Failure("This organization does not exist");

                accessToken = tokenGenerator.GenerateOrganizationAccessToken(
                    currentRefreshToken.User.Id,
                    currentRefreshToken.User.Email,
                    membership.OrganizationId,
                    membership.Role
                );
            }
            else
            {
                accessToken = tokenGenerator.GenerateAccessToken(
                    currentRefreshToken.User.Id,
                    currentRefreshToken.User.Email
                );
            }

            return Result<Response>.Success(new Response(accessToken, newRefreshTokenData));
        }
    }
}
