using System;
using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;

namespace Infrastructure.Authentication;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        return hash;
    }

    public bool VerifyPassword(string password, string hash)
    {
        var isValid = BCrypt.Net.BCrypt.Verify(password, hash);
        return isValid;
    }
}
