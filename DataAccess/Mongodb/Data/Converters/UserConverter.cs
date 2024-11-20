using JustLabel.Models;
using JustLabel.DataMongoDb.Models;

namespace JustLabel.DataMongoDb.Converters;

public static class UserConverter
{
    public static UserDbModel? CoreToDbModel(UserModel? model)
    {
        return model is null ? null : new()
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

    public static UserModel? DbToCoreModel(UserDbModel? model)
    {
        return model is null ? null : new()
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
