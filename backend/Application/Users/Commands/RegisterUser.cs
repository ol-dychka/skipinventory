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
    public record Command(string Email, string Password, string OrganizationId, bool IsOwner) : IRequest<Result<Response>>;

    public class Handler(IUserRepository repository, IJwtTokenGenerator generator, IPasswordHasher hasher) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await repository.GetByEmailAsync(request.Email, cancellationToken);
            if (user != null) return Result<Response>.Failure("Email already in use");

            // password validation
            // email validation

            var refreshTokenData = generator.GenerateRefreshToken();
            var refreshTokenHash = generator.HashRefreshToken(refreshTokenData.Token);
            var passwordHash = hasher.HashPassword(request.Password);

            user = request.IsOwner
                ? User.CreateOwner(request.Email, passwordHash, request.OrganizationId)
                : User.CreateEmployee(request.Email, passwordHash, request.OrganizationId);
            repository.Add(user);

            await repository.SaveChangesAsync(cancellationToken);

            var refreshToken = new RefreshToken(user.Id, refreshTokenHash, refreshTokenData.ExpiresAt);

            await repository.SaveChangesAsync(cancellationToken);

            var accessToken = generator.GenerateAccessToken(user.Id, user.Email);

            return Result<Response>.Success(new Response(accessToken, refreshTokenData.Token)); 
        }
    }
}
