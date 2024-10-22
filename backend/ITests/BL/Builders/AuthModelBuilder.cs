using JustLabel.Models;

namespace IntegrationTests.Builders;

public class AuthModelBuilder
{
    private AuthModel _authModel = new();

    public AuthModelBuilder WithAccessToken(string accessToken)
    {
        _authModel.AccessToken = accessToken;
        return this;
    }

    public AuthModelBuilder WithRefreshToken(string refreshToken)
    {
        _authModel.RefreshToken = refreshToken;
        return this;
    }

    public AuthModel Build()
    {
        return _authModel;
    }
}
