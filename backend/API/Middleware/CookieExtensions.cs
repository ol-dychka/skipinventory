using System;
using Application.Models;

namespace API.Middleware;

public static class CookieExtensions
{
    public static void SetRefreshToken(this IResponseCookies cookies, RefreshTokenData refreshTokenData)
    {
        cookies.Append("refreshToken", refreshTokenData.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Path = "/api/auth",
            Expires = refreshTokenData.ExpiresAt
        });
    }

    public static void DeleteRefreshToken(this IResponseCookies cookies)
    {
        cookies.Delete("refreshToken");
    }
}
