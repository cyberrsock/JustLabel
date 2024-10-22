using JustLabel.Models;

namespace UnitTests.Factories;

public static class AuthModelFactory
{
    public static AuthModel Create(string accessToken, string refreshToken)
    {
        return new AuthModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
