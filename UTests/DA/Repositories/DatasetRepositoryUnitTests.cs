using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using UnitTests.Data;
using UnitTests.Factories;

namespace UnitTests.Repositories;

public class DatasetRepositoryUnitTests
{
    private readonly DatasetRepository _datasetRepository;
    private readonly MockDbContextFactory _mockFactory;

    public DatasetRepositoryUnitTests()
    {
        _mockFactory = new MockDbContextFactory();
        _datasetRepository = new(_mockFactory.MockContext.Object);
    }

    [Fact]
    public void TestAddDatasetInEmptyTable()
    {
        // Arrange
        var dataset = DatasetModelFactory.Create(
            5,
            "Test Dataset",
            "This is a test dataset.",
            3,
            123,
            DateTime.Now
        );

        List<DatasetDbModel> datasets = [];
        _mockFactory.SetDatasetList(datasets);

        // Act
        int last_id = _datasetRepository.Add(dataset);

        // Assert
        Assert.Single(datasets);
        Assert.Equal(dataset.Id, last_id);
        Assert.Equal(dataset.Id, datasets[0].Id);
        Assert.Equal(dataset.Title, datasets[0].Title);
        Assert.Equal(dataset.Description, datasets[0].Description);
        Assert.Equal(dataset.CreatorId, datasets[0].CreatorId);
    }

    [Fact]
    public void TestAddDatasetInNonEmptyTable()
    {
        // Arrange
        var dataset1 = DatasetDbModelFactory.Create(
            123,
            "Test Dataset",
            "This is a test dataset.",
            3,
            DateTime.Now
        );

        var dataset2 = DatasetModelFactory.Create(
            124,
            "Test Dataset",
            "This is a test dataset.",
            3,
            123,
            DateTime.Now
        );

        List<DatasetDbModel> datasets = [dataset1];
        _mockFactory.SetDatasetList(datasets);
        var now = DateTime.Now;

        // Act
        int last_id = _datasetRepository.Add(dataset2);

        // Assert
        Assert.Equal(2, datasets.Count);
        Assert.Equal(124, last_id);
        Assert.Equal(dataset2.Id, datasets[1].Id);
        Assert.Equal(dataset2.Title, datasets[1].Title);
        Assert.Equal(dataset2.Description, datasets[1].Description);
        Assert.Equal(dataset2.CreatorId, datasets[1].CreatorId);
    }

    [Fact]
    public void TestDeleteExistingDataset()
    {
        // Arrange
        var dataset = DatasetModelFactory.Create(
            999,
            "SomeName2",
            "SomeDescription2",
            1,
            2,
            DateTime.Now
        );

        var datasetDbo = DatasetDbModelFactory.Create(dataset);

        List<DatasetDbModel> datasets = [datasetDbo];
        _mockFactory.SetDatasetList(datasets);

        // Act
        _datasetRepository.Delete(dataset.Id);

        // Assert
        Assert.Empty(datasets);
    }

    [Fact]
    public void TestDeleteNonExistentDataset()
    {
        // Arrange
        var datasetDbo = DatasetDbModelFactory.Create(
            1,
            "SomeName1",
            "SomeDescription2",
            2,
            DateTime.Now
        );

        List<DatasetDbModel> datasets = [datasetDbo];
        _mockFactory.SetDatasetList(datasets);

        // Act
        _datasetRepository.Delete(2);

        // Assert
        Assert.Single(datasets);
    }

    [Fact]
    public void TestGetExistingDataset()
    {
        // Arrange
        var datasetDbo = DatasetDbModelFactory.Create(
            999,
            "SomeName",
            "SomeDescription",
            2,
            DateTime.Now
        );

        List<DatasetDbModel> datasets = [datasetDbo];
        _mockFactory.SetDatasetList(datasets);

        // Act
        var resultDataset = _datasetRepository.Get(999);

        // Assert
        Assert.NotNull(resultDataset);
        Assert.Equal(datasetDbo.Id, resultDataset.Id);
        Assert.Equal(datasetDbo.Title, resultDataset.Title);
        Assert.Equal(datasetDbo.Description, resultDataset.Description);
        Assert.Equal(datasetDbo.CreatorId, resultDataset.CreatorId);
    }

    [Fact]
    public void TestGetNonExistentDataset()
    {
        // Arrange
        List<DatasetDbModel> datasets = [];
        _mockFactory.SetDatasetList(datasets);

        // Act
        var addedDataset = _datasetRepository.Get(1);

        // Assert
        Assert.Null(addedDataset);
    }

    [Fact]
    public void TestGetAllDataset()
    {
        // Arrange
        var datasetDbo1 = DatasetDbModelFactory.Create(
            1,
            "SomeName1",
            "SomeDescription1",
            3,
            DateTime.Now
        );

        var datasetDbo2 = DatasetDbModelFactory.Create(
            2,
            "SomeName1",
            "SomeDescription2",
            11,
            DateTime.Now
        );

        List<DatasetDbModel> datasets = [datasetDbo1, datasetDbo2];
        _mockFactory.SetDatasetList(datasets);

        // Act
        var resultDatasets = _datasetRepository.GetAll();

        // Assert
        Assert.Equal(2, resultDatasets.Count);
        Assert.Equal(datasetDbo1.Id, resultDatasets[0].Id);
        Assert.Equal(datasetDbo1.Title, resultDatasets[0].Title);
        Assert.Equal(datasetDbo1.Description, resultDatasets[0].Description);
        Assert.Equal(datasetDbo1.CreatorId, resultDatasets[0].CreatorId);
        Assert.Equal(datasetDbo2.Id, resultDatasets[1].Id);
        Assert.Equal(datasetDbo2.Title, resultDatasets[1].Title);
        Assert.Equal(datasetDbo2.Description, resultDatasets[1].Description);
        Assert.Equal(datasetDbo2.CreatorId, resultDatasets[1].CreatorId);
    }

    [Fact]
    public void TestGetAllNoDataset()
    {
        // Arrange
        List<DatasetDbModel> datasets = [];
        _mockFactory.SetDatasetList(datasets);

        // Act
        var addedDatasets = _datasetRepository.GetAll();

        // Assert
        Assert.Empty(addedDatasets);
    }
}
