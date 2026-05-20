using System;
using Application.Core;
using Application.Interfaces;
using Application.Models;
using Domain;
using MediatR;

namespace Application.Auth.Commands;

public class Register
{
    public record Response(string UserId, string AccessToken, RefreshTokenData RefreshTokenData);

    public record Command(string Email, string Password, string Name) : IRequest<Result<Response>>;

    public class Handler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork,
        ITokenGenerator tokenGenerator,
        IPasswordHasher passwordHasher
    ) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user != null)
                return Result<Response>.Failure("Email already in use");

            // password validation
            // email validation

            var refreshTokenData = tokenGenerator.GenerateRefreshToken();
            var refreshTokenHash = tokenGenerator.HashRefreshToken(refreshTokenData.Token);
            var passwordHash = passwordHasher.HashPassword(request.Password);

            user = new User(request.Email, passwordHash, request.Name);
            userRepository.Add(user);

            var refreshToken = new RefreshToken(
                user.Id,
                refreshTokenHash,
                refreshTokenData.ExpiresAt
            );
            refreshTokenRepository.Add(refreshToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var accessToken = tokenGenerator.GenerateAccessToken(user.Id, user.Email);

            return Result<Response>.Success(new Response(user.Id, accessToken, refreshTokenData));
        }
    }
}
