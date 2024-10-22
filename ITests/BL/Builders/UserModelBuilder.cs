using JustLabel.Models;

namespace IntegrationTests.Builders;

public class UserModelBuilder
{
    private UserModel _userModel = new();

    public UserModelBuilder WithId(int id)
    {
        _userModel.Id = id;
        return this;
    }

    public UserModelBuilder WithUsername(string username)
    {
        _userModel.Username = username;
        return this;
    }

    public UserModelBuilder WithEmail(string email)
    {
        _userModel.Email = email;
        return this;
    }

    public UserModelBuilder WithPassword(string password)
    {
        _userModel.Password = password;
        return this;
    }

    public UserModelBuilder WithSalt(string salt)
    {
        _userModel.Salt = salt;
        return this;
    }

    public UserModelBuilder WithRefreshToken(string refreshToken)
    {
        _userModel.RefreshToken = refreshToken;
        return this;
    }

    public UserModelBuilder WithIsAdmin(bool isAdmin)
    {
        _userModel.IsAdmin = isAdmin;
        return this;
    }

    public UserModelBuilder WithBlockMarks(bool blockMarks)
    {
        _userModel.BlockMarks = blockMarks;
        return this;
    }

    public UserModel Build()
    {
        return _userModel;
    }
}
