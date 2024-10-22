using System;
using System.Collections.Generic;
using JustLabel.Data.Models;

namespace IntegrationTests.Builders;

public class UserDbModelBuilder
{
    private UserDbModel _userDbo = new();

    public UserDbModelBuilder WithId(int id)
    {
        _userDbo.Id = id;
        return this;
    }

    public UserDbModelBuilder WithUsername(string username)
    {
        _userDbo.Username = username;
        return this;
    }

    public UserDbModelBuilder WithEmail(string email)
    {
        _userDbo.Email = email;
        return this;
    }

    public UserDbModelBuilder WithPassword(string password)
    {
        _userDbo.Password = password;
        return this;
    }

    public UserDbModelBuilder WithSalt(string salt)
    {
        _userDbo.Salt = salt;
        return this;
    }

    public UserDbModelBuilder WithRefreshToken(string refreshToken)
    {
        _userDbo.RefreshToken = refreshToken;
        return this;
    }

    public UserDbModelBuilder WithIsAdmin(bool isAdmin)
    {
        _userDbo.IsAdmin = isAdmin;
        return this;
    }

    public UserDbModelBuilder WithBlockMarks(bool blockMarks)
    {
        _userDbo.BlockMarks = blockMarks;
        return this;
    }

    public UserDbModel Build()
    {
        return _userDbo;
    }
}
