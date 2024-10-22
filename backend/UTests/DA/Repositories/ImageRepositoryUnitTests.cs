using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using UnitTests.Data;
using UnitTests.Factories;

namespace UnitTests.Repositories;

public class ImageRepositoryUnitTests
{
    private readonly ImageRepository _imageRepository;
    private readonly MockDbContextFactory _mockFactory;

    public ImageRepositoryUnitTests()
    {
        _mockFactory = new MockDbContextFactory();
        _imageRepository = new (_mockFactory.MockContext.Object);
    }

    [Fact]
    public void TestAddImageInEmptyTable()
    {
        // Arrange
        var image = ImageModelFactory.Create(
            1,
            5,
            "path/to/image.jpg",
            1920,
            1080
        );

        List<ImageDbModel> images = [];
        _mockFactory.SetImageList(images);

        // Act
        _imageRepository.Add(image);

        // Assert
        Assert.Single(images);
        Assert.Equal(image.DatasetId, images[0].DatasetId);
        Assert.Equal(image.Path, images[0].Path);
        Assert.Equal(image.Width, images[0].Width);
        Assert.Equal(image.Height, images[0].Height);
    }

    [Fact]
    public void TestAddImageInNonEmptyTable()
    {
        // Arrange
        var image1 = ImageDbModelFactory.Create(
            1,
            5,
            "path/to/another/image1.jpg",
            1280,
            720
        );

        var image2 = ImageModelFactory.Create(
            2,
            6,
            "path/to/another/image2.jpg",
            1280,
            720
        );

        List<ImageDbModel> images = [image1];
        _mockFactory.SetImageList(images);

        // Act
        _imageRepository.Add(image2);

        // Assert
        Assert.Equal(2, images.Count);
        Assert.Equal(image2.Id, images[1].Id);
        Assert.Equal(image2.DatasetId, images[1].DatasetId);
        Assert.Equal(image2.Path, images[1].Path);
        Assert.Equal(image2.Width, images[1].Width);
        Assert.Equal(image2.Height, images[1].Height);
    }

    [Fact]
    public void TestGetExistingImage()
    {
        // Arrange
        var imageDbo = ImageDbModelFactory.Create(1, 5, "path/to/image.jpg", 1920, 1080);
        
        List<ImageDbModel> images = [imageDbo];
        _mockFactory.SetImageList(images);

        // Act
        var resultImage = _imageRepository.Get(1);

        // Assert
        Assert.NotNull(resultImage);
        Assert.Equal(imageDbo.Id, resultImage.Id);
        Assert.Equal(imageDbo.DatasetId, resultImage.DatasetId);
        Assert.Equal(imageDbo.Path, resultImage.Path);
        Assert.Equal(imageDbo.Width, resultImage.Width);
        Assert.Equal(imageDbo.Height, resultImage.Height);
    }

    [Fact]
    public void TestGetNonExistentImage()
    {
        // Arrange
        List<ImageDbModel> images = [];
        _mockFactory.SetImageList(images);

        // Act
        var resultImage = _imageRepository.Get(1);

        // Assert
        Assert.Null(resultImage);
    }

    [Fact]
    public void TestGetAllImagesForDataset()
    {
        // Arrange
        var imageDbo1 = ImageDbModelFactory.Create(1, 5, "path/to/image1.jpg", 1920, 1080);
        var imageDbo2 = ImageDbModelFactory.Create(2, 5, "path/to/image2.jpg", 1280, 720);
        var imageDbo3 = ImageDbModelFactory.Create(3, 6, "path/to/image2.jpg", 1280, 720);

        List<ImageDbModel> images = [imageDbo1, imageDbo2, imageDbo3];
        _mockFactory.SetImageList(images);

        // Act
        var resultImages = _imageRepository.GetAll(5);

        // Assert
        Assert.Equal(2, resultImages.Count);
        Assert.Equal(imageDbo1.Id, resultImages[0].Id);
        Assert.Equal(imageDbo1.DatasetId, resultImages[0].DatasetId);
        Assert.Equal(imageDbo1.Path, resultImages[0].Path);
        Assert.Equal(imageDbo1.Width, resultImages[0].Width);
        Assert.Equal(imageDbo1.Height, resultImages[0].Height);
        Assert.Equal(imageDbo2.Id, resultImages[1].Id);
        Assert.Equal(imageDbo2.DatasetId, resultImages[1].DatasetId);
        Assert.Equal(imageDbo2.Path, resultImages[1].Path);
        Assert.Equal(imageDbo2.Width, resultImages[1].Width);
        Assert.Equal(imageDbo2.Height, resultImages[1].Height);
    }

    [Fact]
    public void TestGetAllImagesNoImagesForDataset()
    {
        // Arrange
        List<ImageDbModel> images = [];
        _mockFactory.SetImageList(images);

        // Act
        var resultImages = _imageRepository.GetAll(5);

        // Assert
        Assert.Empty(resultImages);
    }
}
