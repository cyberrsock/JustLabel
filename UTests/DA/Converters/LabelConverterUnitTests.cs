using Xunit;
using JustLabel.Data.Converters;
using UnitTests.Builders;

namespace UnitTests.Converters;

public class LabelConverterUnitTests
{
    [Fact]
    public void TestConvertOkCoreToDbModel()
    {
        // Arrange
        var label = new LabelModelBuilder()
            .WithId(1)
            .WithTitle("Sample Label")
            .Build();

        // Act
        var labelDb = LabelConverter.CoreToDbModel(label);

        // Assert
        Assert.Equal(label.Id, labelDb.Id);
        Assert.Equal(label.Title, labelDb.Title);
    }

    [Fact]
    public void TestConvertOkDbToCoreModel()
    {
        // Arrange
        var labelDb = new LabelDbModelBuilder()
            .WithId(1)
            .WithTitle("Sample Label")
            .Build();

        // Act
        var label = LabelConverter.DbToCoreModel(labelDb);

        // Assert
        Assert.Equal(labelDb.Id, label.Id);
        Assert.Equal(labelDb.Title, label.Title);
    }

    [Fact]
    public void TestConvertNullCoreToDbModel()
    {
        // Arrange

        // Act
        var labelDb = LabelConverter.CoreToDbModel(null);

        // Assert
        Assert.Null(labelDb);
    }

    [Fact]
    public void TestConvertNullDbToCoreModel()
    {
        // Arrange

        // Act
        var label = LabelConverter.DbToCoreModel(null);

        // Assert
        Assert.Null(label);
    }
}
