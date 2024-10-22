using Xunit;
using JustLabel.Data.Converters;
using UnitTests.Builders;

namespace UnitTests.Converters;

public class DatasetConverterUnitTests
{
    [Fact]
    public void TestConvertOkCoreToDbModel()
    {
        // Arrange
        var dataset = new DatasetModelBuilder()
            .WithId(1)
            .WithTitle("Test Dataset")
            .WithDescription("This is a test dataset.")
            .WithCreatorId(123)
            .WithLoadDatetime(DateTime.Now)
            .Build();

        // Act
        var datasetDb = DatasetConverter.CoreToDbModel(dataset);

        // Assert
        Assert.Equal(dataset.Id, datasetDb.Id);
        Assert.Equal(dataset.Title, datasetDb.Title);
        Assert.Equal(dataset.Description, datasetDb.Description);
        Assert.Equal(dataset.CreatorId, datasetDb.CreatorId);
        Assert.Equal(dataset.LoadDatetime, datasetDb.LoadDatetime);
    }

    [Fact]
    public void TestConvertOkDbToCoreModel()
    {
        // Arrange
        var datasetDb = new DatasetDbModelBuilder()
            .WithId(1)
            .WithTitle("Test Dataset")
            .WithDescription("This is a test dataset.")
            .WithCreatorId(123)
            .WithLoadDatetime(DateTime.Now)
            .Build();

        // Act
        var dataset = DatasetConverter.DbToCoreModel(datasetDb);

        // Assert
        Assert.Equal(datasetDb.Id, dataset.Id);
        Assert.Equal(datasetDb.Title, dataset.Title);
        Assert.Equal(datasetDb.Description, dataset.Description);
        Assert.Equal(datasetDb.CreatorId, dataset.CreatorId);
        Assert.Equal(datasetDb.LoadDatetime, dataset.LoadDatetime);
    }

    [Fact]
    public void TestConvertNullCoreToDbModel()
    {
        // Arrange

        // Act
        var datasetDb = DatasetConverter.CoreToDbModel(null);

        // Assert
        Assert.Null(datasetDb);
    }

    [Fact]
    public void TestConvertNullDbToCoreModel()
    {
        // Arrange

        // Act
        var dataset = DatasetConverter.DbToCoreModel(null);

        // Assert
        Assert.Null(dataset);
    }
}
