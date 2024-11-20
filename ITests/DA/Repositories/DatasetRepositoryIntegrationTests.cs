using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using IntegrationTests.Data;
using IntegrationTests.Factories;
using IntegrationTests.Builders;

namespace IntegrationTests.Repositories;

[Collection("Test Database")]
public class DatasetRepositoryIntegrationTests : BaseRepositoryIntegrationTests
{
    private readonly DatasetRepository _datasetRepository;

    public DatasetRepositoryIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _datasetRepository = new(Fixture.CreateContext());
    }

    private JustLabel.Data.AppDbContext Initialize()
    {
        var context = Fixture.CreateContext();

        var user1 = new UserDbModelBuilder()
            .WithId(1)
            .Build();

        var user2 = new UserDbModelBuilder()
            .WithId(2)
            .Build();

        var user3 = new UserDbModelBuilder()
            .WithId(3)
            .Build();

        context.Users.Add(user1);
        context.Users.Add(user2);
        context.Users.Add(user3);
        context.SaveChanges();

        return context;
    }

    [Fact]
    public void TestAddDatasetInEmptyTable()
    {
        using var context = Initialize();

        // Arrange
        var dataset = DatasetModelFactory.Create(
            5,
            "Test Dataset",
            "This is a test dataset.",
            3,
            1,
            DateTime.Now
        );

        context.SaveChanges();

        // Act
        int last_id = _datasetRepository.Add(dataset);

        // Assert
        var datasets = (from d in context.Datasets select d).ToList();
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
        using var context = Initialize();

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
            2,
            DateTime.Now
        );

        var now = DateTime.Now;

        context.Datasets.Add(dataset1);
        context.SaveChanges();

        // Act
        int last_id = _datasetRepository.Add(dataset2);

        // Assert
        var datasets = (from d in context.Datasets select d).ToList();
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
        using var context = Initialize();

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
        context.SaveChanges();

        // Act
        _datasetRepository.Delete(dataset.Id);

        // Assert
        var datasets = (from d in context.Datasets select d).ToList();
        Assert.Empty(datasets);
    }

    [Fact]
    public void TestDeleteNonExistentDataset()
    {
        using var context = Initialize();

        // Arrange
        var datasetDbo = DatasetDbModelFactory.Create(
            1,
            "SomeName1",
            "SomeDescription2",
            2,
            DateTime.Now
        );

        context.Datasets.Add(datasetDbo);
        context.SaveChanges();

        // Act
        _datasetRepository.Delete(2);

        // Assert
        var datasets = (from d in context.Datasets select d).ToList();
        Assert.Single(datasets);
    }

    [Fact]
    public void TestGetExistingDataset()
    {
        using var context = Initialize();

        // Arrange
        var datasetDbo = DatasetDbModelFactory.Create(
            999,
            "SomeName",
            "SomeDescription",
            2,
            DateTime.Now
        );

        context.Datasets.Add(datasetDbo);
        context.SaveChanges();

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
        using var context = Initialize();

        // Arrange

        // Act
        var addedDataset = _datasetRepository.Get(1);

        // Assert
        Assert.Null(addedDataset);
    }

    [Fact]
    public void TestGetAllDataset()
    {
        using var context = Initialize();

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
            2,
            DateTime.Now
        );

        context.Datasets.Add(datasetDbo1);
        context.Datasets.Add(datasetDbo2);
        context.SaveChanges();

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
        using var context = Initialize();

        // Arrange

        // Act
        var addedDatasets = _datasetRepository.GetAll();

        // Assert
        Assert.Empty(addedDatasets);
    }
}
