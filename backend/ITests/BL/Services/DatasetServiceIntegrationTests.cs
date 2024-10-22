using Xunit;
using Moq;
using JustLabel.Data;
using JustLabel.Models;
using JustLabel.Exceptions;
using JustLabel.Repositories;
using JustLabel.Services;
using IntegrationTests.Data;
using IntegrationTests.Builders;

namespace IntegrationTests.Services;

[Collection("Test Database")]
public class DatasetServiceIntegrationTests: BaseServiceIntegrationTests
{
    private readonly AppDbContext _context;
    private readonly DatasetService _datasetService;
    private readonly DatasetRepository _datasetRepository;
    private readonly UserRepository _userRepository;
    private readonly ImageRepository _imageRepository;
    private readonly MarkedRepository _markedRepository;

    public DatasetServiceIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _context = Fixture.CreateContext();
        _datasetRepository = new(_context);
        _userRepository = new(_context);
        _imageRepository = new(_context);
        _markedRepository = new(_context);
        _datasetService = new DatasetService(
            _datasetRepository,
            _userRepository,
            _imageRepository,
            _markedRepository
        );
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
    public void TestAddDatasetWithOkTitle()
    {
        using var context = Initialize();

        // Arrange
        var dataset = new DatasetModelBuilder()
            .WithId(1)
            .WithTitle("Test Dataset")
            .WithDescription("This is a test dataset.")
            .WithImageCount(2)
            .WithCreatorId(3)
            .Build();

        var image1 = new ImageModelBuilder()
            .WithId(1)
            .WithDatasetId(1)
            .WithPath("path/to/image1.jpg")
            .WithWidth(1920)
            .WithHeight(1080)
            .Build();
        var image2 = new ImageModelBuilder()
            .WithId(2)
            .WithDatasetId(1)
            .WithPath("path/to/image2.jpg")
            .WithWidth(1920)
            .WithHeight(1080)
            .Build();

        List<ImageModel> imageGroup = [image1, image2];

        // Act
        int datasetId = _datasetService.Create(dataset, imageGroup);

        // Assert
        var datasets = (from d in context.Datasets select d).ToList();
        var images = (from i in context.Images select i).ToList();
        Assert.Equal(1, datasetId);
        Assert.Single(datasets);
        Assert.Equal(2, images.Count);
        Assert.Equal(dataset.Id, datasets[0].Id);
        Assert.Equal(dataset.Title, datasets[0].Title);
        Assert.Equal(dataset.Description, datasets[0].Description);
        Assert.Equal(dataset.CreatorId, datasets[0].CreatorId);
        Assert.Equal(image1.Id, images[0].Id);
        Assert.Equal(image1.DatasetId, images[0].DatasetId);
        Assert.Equal(image1.Path, images[0].Path);
        Assert.Equal(image1.Width, images[0].Width);
        Assert.Equal(image1.Height, images[0].Height);
        Assert.Equal(image2.Id, images[1].Id);
        Assert.Equal(image2.DatasetId, images[1].DatasetId);
        Assert.Equal(image2.Path, images[1].Path);
        Assert.Equal(image2.Width, images[1].Width);
        Assert.Equal(image2.Height, images[1].Height);
    }

    [Fact]
    public void TestAddDatasetWithEmptyTitle()
    {
        using var context = Initialize();

        // Arrange
        var dataset = new DatasetModelBuilder()
            .WithId(1)
            .WithDescription("This is a test dataset.")
            .WithCreatorId(3)
            .Build();

        // Act
        var exception = Assert.Throws<FailedDatasetCreationException>(() => _datasetService.Create(dataset, []));

        // Assert
        var datasets = (from d in context.Datasets select d).ToList();
        var images = (from i in context.Images select i).ToList();
        Assert.Equal("Title field cannot be empty", exception.Message);
        Assert.Empty(datasets);
        Assert.Empty(images);
    }

    [Fact]
    public void TestGetDatasetWithExistingId()
    {
        using var context = Initialize();

        // Arrange
        int datasetId = 1;
        var dataset = new DatasetDbModelBuilder()
            .WithId(datasetId)
            .WithCreatorId(1)
            .WithTitle("Test Dataset")
            .Build();

        context.Datasets.Add(dataset);
        context.Images.Add(new ImageDbModelBuilder().WithId(1).WithDatasetId(datasetId).Build());
        context.SaveChanges();

        // Act
        var (foundDataset, foundImages) = _datasetService.Get(datasetId);

        // Assert
        var images = (from i in context.Images select i).ToList();
        Assert.Equal(datasetId, foundDataset.Id);
        Assert.Equal(images.Count, foundImages.Count);
    }

    [Fact]
    public void TestGetDatasetWithNonExistingId()
    {
        using var context = Initialize();

        // Arrange
        int datasetId = 1;
        
        // Act
        var exception = Assert.Throws<DatasetNotExitedException>(() => _datasetService.Get(datasetId));

        // Assert
        Assert.Equal("Dataset with this id does not exist", exception.Message);
    }

    [Fact]
    public void TestGetAllDatasets()
    {
        using var context = Initialize();

        // Arrange
        var dataset1 = new DatasetDbModelBuilder().WithId(1).WithCreatorId(1).WithTitle("Dataset 1").Build();
        var dataset2 = new DatasetDbModelBuilder().WithId(2).WithCreatorId(1).WithTitle("Dataset 2").Build();

        context.Datasets.Add(dataset1);
        context.Datasets.Add(dataset2);
        context.SaveChanges();

        // Act
        var result = _datasetService.Get();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(dataset1.Title, result[0].Title);
        Assert.Equal(dataset2.Title, result[1].Title);
    }

    [Fact]
    public void TestGetAllDatasetsWhenNoDatasetsAvailable()
    {
        using var context = Initialize();

        // Arrange

        // Act
        var result = _datasetService.Get();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void TestWhichImageExists()
    {
        using var context = Initialize();

        // Arrange
        var dataset1 = new DatasetDbModelBuilder().WithId(1).WithCreatorId(1).WithTitle("Dataset 1").Build();
        context.Datasets.Add(dataset1);

        int imageId = 1;
        var image = new ImageDbModelBuilder().WithId(imageId).WithDatasetId(1).Build();
        context.Images.Add(image);
        context.SaveChanges();

        // Act
        var resultDatasetId = _datasetService.WhichImage(imageId);

        // Assert
        Assert.Equal(1, resultDatasetId);
    }

    [Fact]
    public void TestWhichImageNotExists()
    {
        using var context = Initialize();

        // Arrange
        int imageId = 1;
        
        // Act
        var resultDatasetId = _datasetService.WhichImage(imageId);

        // Assert
        Assert.Equal(-1, resultDatasetId);
    }

    [Fact]
    public void TestDeleteDatasetSuccessful()
    {
        using var context = Initialize();

        // Arrange
        int datasetId = 1;
        var dataset = new DatasetDbModelBuilder().WithId(datasetId).WithCreatorId(2).Build();        
        context.Datasets.Add(dataset);
        context.SaveChanges();

        // Act
        _datasetService.Delete(datasetId);

        // Assert
        var datasets = (from d in context.Datasets select d).ToList();
        Assert.Empty(datasets);
    }


    [Fact]
    public void TestDeleteNonExistingDatasetFails()
    {
        using var context = Initialize();

        // Arrange
        int datasetId = 1;

        // Act
        var exception = Assert.Throws<DatasetNotExitedException>(() => _datasetService.Delete(datasetId));

        // Assert
        Assert.Equal("Dataset with this id does not exist", exception.Message);
    }
}
