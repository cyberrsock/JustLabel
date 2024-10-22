using System;
using JustLabel.Models;
using JustLabel.Data.Models;

namespace IntegrationTests.Factories;

public static class UserDbModelFactory
{
    public static UserDbModel Create(
        int id,
        string username,
        string email,
        string password,
        string salt,
        string refreshToken,
        bool isAdmin,
        bool blockMarks)
    {
        return new UserDbModel
        {
            Id = id,
            Username = username,
            Email = email,
            Password = password,
            Salt = salt,
            RefreshToken = refreshToken,
            IsAdmin = isAdmin,
            BlockMarks = blockMarks
        };
    }

    public static UserDbModel Create(UserModel model)
    {
        return new UserDbModel
        {
            Id = model.Id,
            Username = model.Username,
            Email = model.Email,
            Password = model.Password,
            Salt = model.Salt,
            RefreshToken = model.RefreshToken,
            IsAdmin = model.IsAdmin,
            BlockMarks = model.BlockMarks
        };
    }
}
