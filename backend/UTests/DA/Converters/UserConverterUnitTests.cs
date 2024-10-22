using Xunit;
using JustLabel.Data.Converters;
using UnitTests.Builders;

namespace UnitTests.Converters;

public class UserConverterUnitTests
{
    [Fact]
    public void TestConvertOkCoreToDbModel()
    {
        // Arrange
        var user = new UserModelBuilder()
            .WithId(1)
            .WithUsername("testuser")
            .WithEmail("testuser@example.com")
            .WithPassword("securepassword")
            .WithSalt("randomsalt")
            .WithRefreshToken("dummytoken")
            .WithIsAdmin(false)
            .WithBlockMarks(true)
            .Build();

        // Act
        var userDb = UserConverter.CoreToDbModel(user);

        // Assert
        Assert.Equal(user.Id, userDb.Id);
        Assert.Equal(user.Username, userDb.Username);
        Assert.Equal(user.Email, userDb.Email);
        Assert.Equal(user.Password, userDb.Password);
        Assert.Equal(user.Salt, userDb.Salt);
        Assert.Equal(user.RefreshToken, userDb.RefreshToken);
        Assert.Equal(user.IsAdmin, userDb.IsAdmin);
        Assert.Equal(user.BlockMarks, userDb.BlockMarks);
    }

    [Fact]
    public void TestConvertOkDbToCoreModel()
    {
        // Arrange
        var userDb = new UserDbModelBuilder()
            .WithId(1)
            .WithUsername("testuser")
            .WithEmail("testuser@example.com")
            .WithPassword("securepassword")
            .WithSalt("randomsalt")
            .WithRefreshToken("dummytoken")
            .WithIsAdmin(false)
            .WithBlockMarks(true)
            .Build();

        // Act
        var user = UserConverter.DbToCoreModel(userDb);

        // Assert
        Assert.Equal(userDb.Id, user.Id);
        Assert.Equal(userDb.Username, user.Username);
        Assert.Equal(userDb.Email, user.Email);
        Assert.Equal(userDb.Password, user.Password);
        Assert.Equal(userDb.Salt, user.Salt);
        Assert.Equal(userDb.RefreshToken, user.RefreshToken);
        Assert.Equal(userDb.IsAdmin, user.IsAdmin);
        Assert.Equal(userDb.BlockMarks, user.BlockMarks);
    }

    [Fact]
    public void TestConvertNullCoreToDbModel()
    {
        // Arrange

        // Act
        var userDb = UserConverter.CoreToDbModel(null);

        // Assert
        Assert.Null(userDb);
    }

    [Fact]
    public void TestConvertNullDbToCoreModel()
    {
        // Arrange

        // Act
        var user = UserConverter.DbToCoreModel(null);

        // Assert
        Assert.Null(user);
    }
}
