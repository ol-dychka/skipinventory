using System;
using Application.Models;
using Domain;

namespace Application.Interfaces;

public interface ITokenGenerator
{
    string GenerateAccessToken(string id, string email);
    string GenerateOrganizationAccessToken(
        string id,
        string email,
        string organizationId,
        string role
    );
    RefreshTokenData GenerateRefreshToken();
    string HashRefreshToken(string token);
}
