using System;

namespace API.Middleware;

public static class CookieExtensions
{
    public static void SetRefreshToken(this IResponseCookies cookies, string token)
    {
        cookies.Append("refreshToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/api/auth"
        });
    }

    public static void DeleteRefreshToken(this IResponseCookies cookies)
    {
        cookies.Delete("refreshToken");
    }
}
