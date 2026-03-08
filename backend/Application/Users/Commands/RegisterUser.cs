using System;
using System.Runtime.Intrinsics.Arm;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.Win32.SafeHandles;

namespace Application.Users.Commands;

public class RegisterUser
{
    public record Response(string AccessToken, string RefreshToken);
    public record Command(string Email, string Password, string Name, bool IsOwner) : IRequest<Result<Response>>;

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
            if (user != null) return Result<Response>.Failure("Email already in use");

            // password validation
            // email validation

            var refreshTokenData = tokenGenerator.GenerateRefreshToken();
            var refreshTokenHash = tokenGenerator.HashRefreshToken(refreshTokenData.Token);
            var passwordHash = passwordHasher.HashPassword(request.Password);

            user = request.IsOwner
                ? User.CreateOwner(request.Email, passwordHash, request.Name)
                : User.CreateEmployee(request.Email, passwordHash, request.Name);
            userRepository.Add(user);

            await userRepository.SaveChangesAsync(cancellationToken);

            var refreshToken = new RefreshToken(user.Id, refreshTokenHash, refreshTokenData.ExpiresAt);
            
            refreshTokenRepository.Add(refreshToken);

            await refreshTokenRepository.SaveChangesAsync(cancellationToken);

            var accessToken = tokenGenerator.GenerateAccessToken(user.Id, user.Email);

            return Result<Response>.Success(new Response(accessToken, refreshTokenData.Token)); 
        }
    }
}
