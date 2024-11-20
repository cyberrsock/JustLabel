using Xunit;
using Moq;
using JustLabel.Models;
using JustLabel.Exceptions;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services;
using UnitTests.Builders;

namespace UnitTests.Services;

public class DatasetServiceUnitTests
{
    private readonly DatasetService _datasetService;
    private readonly Mock<IDatasetRepository> _mockDatasetRepository = new();
    private readonly Mock<IImageRepository> _mockImageRepository = new();
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly Mock<IMarkedRepository> _mockMarkedRepository = new();

    public DatasetServiceUnitTests()
    {
        _datasetService = new DatasetService(
            _mockDatasetRepository.Object,
            _mockUserRepository.Object,
            _mockImageRepository.Object,
            _mockMarkedRepository.Object
        );
    }

    [Fact]
    public void TestAddDatasetWithOkTitle()
    {
        // Arrange
        var dataset = new DatasetModelBuilder()
            .WithId(1)
            .WithTitle("Test Dataset")
            .WithDescription("This is a test dataset.")
            .WithImageCount(2)
            .WithCreatorId(3)
            .Build();

        List<DatasetModel> datasets = [];

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

        List<ImageModel> images = [];

        _mockUserRepository
            .Setup(s => s.GetUserById(dataset.CreatorId))
            .Returns(
                new UserModelBuilder()
                    .WithId(3)
                    .Build()
            );
        _mockDatasetRepository
            .Setup(s => s.Add(It.IsAny<DatasetModel>()))
            .Callback((DatasetModel d) => datasets.Add(d))
            .Returns((DatasetModel d) => d.Id);
        _mockImageRepository
            .Setup(s => s.Add(It.IsAny<ImageModel>()))
            .Callback((ImageModel i) => images.Add(i));

        // Act
        int datasetId = _datasetService.Create(dataset, imageGroup);

        // Assert
        Assert.Equal(1, datasetId);
        Assert.Single(datasets);
        Assert.Equal(2, images.Count);
        Assert.Equal(dataset.Id, datasets[0].Id);
        Assert.Equal(dataset.Title, datasets[0].Title);
        Assert.Equal(dataset.Description, datasets[0].Description);
        Assert.Equal(dataset.CreatorId, datasets[0].CreatorId);
        Assert.Equal(dataset.ImageCount, datasets[0].ImageCount);
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
        // Arrange
        var dataset = new DatasetModelBuilder()
            .WithId(1)
            .WithDescription("This is a test dataset.")
            .WithCreatorId(3)
            .Build();

        List<DatasetModel> datasets = [];

        List<ImageModel> imageGroup = [];

        List<ImageModel> images = [];

        _mockUserRepository
            .Setup(s => s.GetUserById(dataset.CreatorId))
            .Returns(
                new UserModelBuilder()
                    .WithId(3)
                    .Build()
            );
        _mockDatasetRepository
            .Setup(s => s.Add(It.IsAny<DatasetModel>()))
            .Callback((DatasetModel d) => datasets.Add(d))
            .Returns((DatasetModel d) => d.Id);
        _mockImageRepository
            .Setup(s => s.Add(It.IsAny<ImageModel>()))
            .Callback((ImageModel i) => images.Add(i));

        // Act
        var exception = Assert.Throws<FailedDatasetCreationException>(() => _datasetService.Create(dataset, imageGroup));

        // Assert
        Assert.Equal("Title field cannot be empty", exception.Message);
        Assert.Empty(datasets);
        Assert.Empty(images);
    }

    [Fact]
    public void TestGetDatasetWithExistingId()
    {
        // Arrange
        int datasetId = 1;
        var dataset = new DatasetModelBuilder()
            .WithId(datasetId)
            .WithTitle("Test Dataset")
            .Build();

        var images = new List<ImageModel>
        {
            new ImageModelBuilder().WithId(1).WithDatasetId(datasetId).Build(),
        };

        _mockDatasetRepository
            .Setup(s => s.Get(datasetId))
            .Returns(dataset);
        _mockImageRepository
            .Setup(s => s.GetAll(datasetId))
            .Returns(images);

        // Act
        var (foundDataset, foundImages) = _datasetService.Get(datasetId);

        // Assert
        Assert.Equal(datasetId, foundDataset.Id);
        Assert.Equal(images.Count, foundImages.Count);
    }

    [Fact]
    public void TestGetDatasetWithNonExistingId()
    {
        // Arrange
        int datasetId = 1;

        _mockDatasetRepository
            .Setup(s => s.Get(datasetId))
            .Returns((DatasetModel)null);

        // Act
        var exception = Assert.Throws<DatasetNotExitedException>(() => _datasetService.Get(datasetId));

        // Assert
        Assert.Equal("Dataset with this id does not exist", exception.Message);
    }

    [Fact]
    public void TestGetAllDatasets()
    {
        // Arrange
        var datasetList = new List<DatasetModel>
        {
            new DatasetModelBuilder().WithId(1).WithTitle("Dataset 1").Build(),
            new DatasetModelBuilder().WithId(2).WithTitle("Dataset 2").Build(),
        };

        _mockDatasetRepository.Setup(s => s.GetAll()).Returns(datasetList);

        // Act
        var result = _datasetService.Get();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(datasetList[0].Title, result[0].Title);
        Assert.Equal(datasetList[1].Title, result[1].Title);
    }

    [Fact]
    public void TestGetAllDatasetsWhenNoDatasetsAvailable()
    {
        // Arrange
        _mockDatasetRepository.Setup(s => s.GetAll()).Returns([]);

        // Act
        var result = _datasetService.Get();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void TestWhichImageExists()
    {
        // Arrange
        int imageId = 1;
        var image = new ImageModelBuilder().WithId(imageId).WithDatasetId(2).Build();

        _mockImageRepository.Setup(s => s.Get(imageId)).Returns(image);

        // Act
        var resultDatasetId = _datasetService.WhichImage(imageId);

        // Assert
        Assert.Equal(2, resultDatasetId);
    }

    [Fact]
    public void TestWhichImageNotExists()
    {
        // Arrange
        int imageId = 1;

        _mockImageRepository.Setup(s => s.Get(imageId)).Returns((ImageModel)null);

        // Act
        var resultDatasetId = _datasetService.WhichImage(imageId);

        // Assert
        Assert.Equal(-1, resultDatasetId);
    }

    [Fact]
    public void TestDeleteDatasetSuccessful()
    {
        // Arrange
        int datasetId = 1;
        var dataset = new DatasetModelBuilder().WithId(datasetId).Build();

        List<DatasetModel> datasets = [dataset];

        _mockDatasetRepository
            .Setup(s => s.Get(datasetId))
            .Returns(dataset);
        _mockDatasetRepository
            .Setup(s => s.Delete(It.IsAny<int>()))
            .Callback((int id) => datasets.RemoveAll(d => d.Id == id));
        _mockMarkedRepository.Setup(s => s.Get_By_DatasetId(datasetId)).Returns([]);

        // Act
        _datasetService.Delete(datasetId);

        // Assert
        _mockDatasetRepository.Verify(s => s.Delete(datasetId), Times.Once);
        Assert.Empty(datasets);
    }


    [Fact]
    public void TestDeleteNonExistingDatasetFails()
    {
        // Arrange
        int datasetId = 1;

        _mockDatasetRepository.Setup(s => s.Get(datasetId)).Returns((DatasetModel)null);

        // Act
        var exception = Assert.Throws<DatasetNotExitedException>(() => _datasetService.Delete(datasetId));

        // Assert
        Assert.Equal("Dataset with this id does not exist", exception.Message);
    }
}