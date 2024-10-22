using Xunit;
using JustLabel.Data.Converters;
using UnitTests.Builders;

namespace UnitTests.Converters;

public class BannedConverterUnitTests
{
    [Fact]
    public void TestConvertOkCoreToDbModel()
    {
        // Arrange
        var banned = new BannedModelBuilder()
            .WithId(1)
            .WithUserId(123)
            .WithAdminId(456)
            .WithReason("Violation of terms")
            .WithBanDatetime(DateTime.Now)
            .Build();

        // Act
        var bannedDb = BannedConverter.CoreToDbModel(banned);

        // Assert
        Assert.Equal(banned.Id, bannedDb.Id);
        Assert.Equal(banned.UserId, bannedDb.UserId);
        Assert.Equal(banned.AdminId, bannedDb.AdminId);
        Assert.Equal(banned.Reason, bannedDb.Reason);
        Assert.Equal(banned.BanDatetime, bannedDb.BanDatetime);
    }

    [Fact]
    public void TestConvertOkDbToCoreModel()
    {
        // Arrange
        var bannedDb = new BannedDbModelBuilder()
            .WithId(1)
            .WithUserId(123)
            .WithAdminId(456)
            .WithReason("Violation of terms")
            .WithBanDatetime(DateTime.Now)
            .Build();

        // Act
        var banned = BannedConverter.DbToCoreModel(bannedDb);

        // Assert
        Assert.Equal(bannedDb.Id, banned.Id);
        Assert.Equal(bannedDb.UserId, banned.UserId);
        Assert.Equal(bannedDb.AdminId, banned.AdminId);
        Assert.Equal(bannedDb.Reason, banned.Reason);
        Assert.Equal(bannedDb.BanDatetime, banned.BanDatetime);
    }

    [Fact]
    public void TestConvertNullCoreToDbModel()
    {
        // Arrange

        // Act
        var bannedDb = BannedConverter.CoreToDbModel(null);

        // Assert
        Assert.Null(bannedDb);
    }

    [Fact]
    public void TestConvertNullDbToCoreModel()
    {
        // Arrange

        // Act
        var banned = BannedConverter.DbToCoreModel(null);

        // Assert
        Assert.Null(banned);
    }
}
