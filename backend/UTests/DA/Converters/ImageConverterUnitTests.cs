using Xunit;
using JustLabel.Data.Converters;
using UnitTests.Builders;
using System.Linq;

namespace UnitTests.Converters;

public class ImageConverterUnitTests
{
    [Fact]
    public void TestConvertOkCoreToDbModel()
    {
        // Arrange
        var image = new ImageModelBuilder()
            .WithId(1)
            .WithDatasetId(2)
            .WithPath("example/path/to/image.jpg")
            .WithWidth(640)
            .WithHeight(480)
            .Build();

        // Act
        var imageDb = ImageConverter.CoreToDbModel(image);

        // Assert
        Assert.Equal(image.Id, imageDb.Id);
        Assert.Equal(image.DatasetId, imageDb.DatasetId);
        Assert.Equal(image.Path, imageDb.Path);
        Assert.Equal(image.Width, imageDb.Width);
        Assert.Equal(image.Height, imageDb.Height);
    }

    [Fact]
    public void TestConvertOkDbToCoreModel()
    {
        // Arrange
        var imageDb = new ImageDbModelBuilder()
            .WithId(1)
            .WithDatasetId(2)
            .WithPath("example/path/to/image.jpg")
            .WithWidth(640)
            .WithHeight(480)
            .Build();

        // Act
        var image = ImageConverter.DbToCoreModel(imageDb);

        // Assert
        Assert.Equal(imageDb.Id, image.Id);
        Assert.Equal(imageDb.DatasetId, image.DatasetId);
        Assert.Equal(imageDb.Path, image.Path);
        Assert.Equal(imageDb.Width, image.Width);
        Assert.Equal(imageDb.Height, image.Height);
    }

    [Fact]
    public void TestConvertNullCoreToDbModel()
    {
        // Arrange

        // Act
        var imageDb = ImageConverter.CoreToDbModel(null);

        // Assert
        Assert.Null(imageDb);
    }

    [Fact]
    public void TestConvertNullDbToCoreModel()
    {
        // Arrange

        // Act
        var image = ImageConverter.DbToCoreModel(null);

        // Assert
        Assert.Null(image);
    }
}
