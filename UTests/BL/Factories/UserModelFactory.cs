using JustLabel.Models;
using JustLabel.Data.Models;

namespace UnitTests.Factories;

public static class UserModelFactory
{
    public static UserModel Create(
        int id,
        string username,
        string email,
        string password,
        string salt,
        string refreshToken,
        bool isAdmin,
        bool blockMarks)
    {
        return new UserModel
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

    public static UserModel Create(UserDbModel model)
    {
        return new UserModel
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
