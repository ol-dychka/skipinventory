using System;
using Application.Models;
using Domain;

namespace Application.Interfaces;

public interface ITokenGenerator
{
    string GenerateAccessToken(string id, string email);
    RefreshTokenData GenerateRefreshToken();
    string HashRefreshToken(string token);
}
