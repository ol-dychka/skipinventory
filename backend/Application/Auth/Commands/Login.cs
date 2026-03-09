using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Auth.Commands;

public class Login
{
    public record Response(string AccessToken, string RefreshToken);
    public record Command(string Email, string Password ) : IRequest<Result<Response>>;

    public class Handler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenGenerator tokenGenerator,
        IPasswordHasher passwordHasher
    ) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null) return Result<Response>.Failure("Email not found");

            var isValidPassword = passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!isValidPassword) return Result<Response>.Failure("Password is not correct");

            var refreshTokenData = tokenGenerator.GenerateRefreshToken();
            var refreshTokenHash = tokenGenerator.HashRefreshToken(refreshTokenData.Token);

            var refreshToken = new RefreshToken(user.Id, refreshTokenHash, refreshTokenData.ExpiresAt);
            
            refreshTokenRepository.Add(refreshToken);

            await refreshTokenRepository.SaveChangesAsync(cancellationToken);

            var accessToken = tokenGenerator.GenerateAccessToken(user.Id, user.Email);

            return Result<Response>.Success(new Response(accessToken, refreshTokenData.Token)); 
        }
    }
}
