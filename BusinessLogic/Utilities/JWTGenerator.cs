using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace JustLabel.Utilities;

public static class JWTGenerator
{
    private static readonly string secretKey = "superkey12345678superkey12345678";

    public static string GenerateAccessToken(int id, bool isAdmin)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var isAdminClaim = isAdmin ? new System.Security.Claims.Claim("role", "admin") : new System.Security.Claims.Claim("role", "user");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim("id", id.ToString()),
                isAdminClaim
            }),
            Expires = DateTime.UtcNow.AddMinutes(900),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public static string GenerateRefreshToken(string accessToken)
    {
        DateTime dateTime = DateTime.UtcNow.AddMinutes(60);
        int year = dateTime.Year % 100;
        int month = dateTime.Month;
        int day = dateTime.Day;
        int hour = dateTime.Hour;
        int minute = dateTime.Minute;

        uint date = (uint)((year << 20) | (month << 16) | (day << 11) | (hour << 6) | minute);

        StringBuilder sBuilder = new();
        for (int i = 30; i >= 0; i -= 6)
        {
            byte b6Bit = (byte)((date >> i) & 0x3F);
            char asciiChar = (char)(b6Bit + 48);
            sBuilder.Append(asciiChar);
        }

        string tokenExpirationDate = sBuilder.ToString();
        string randomData = Guid.NewGuid().ToString("N")[..12];
        string accessTokenLastPart = accessToken[^6..];

        return tokenExpirationDate + randomData + accessTokenLastPart;
    }

    public static int ValidateAccessToken(string accessToken, bool validateLifetime = true)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

        try
        {
            var principal = tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = validateLifetime,
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var idClaim = principal.FindFirst("id");
            if (idClaim != null && int.TryParse(idClaim.Value, out int id))
            {
                return id;
            }
            return -1;
        }
        catch
        {
            return -1;
        }
    }

    public static bool ValidateRefreshToken(string refreshToken, string accessToken)
    {
        string refreshTokenLastPart = refreshToken[^6..];
        string accessTokenLastPart = accessToken[^6..];

        return refreshTokenLastPart == accessTokenLastPart;
    }
}
