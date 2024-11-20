using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using IntegrationTests.Data;
using IntegrationTests.Factories;
using IntegrationTests.Builders;

namespace IntegrationTests.Repositories;

[Collection("Test Database")]
public class ImageRepositoryIntegrationTests : BaseRepositoryIntegrationTests
{
    private readonly ImageRepository _imageRepository;

    public ImageRepositoryIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _imageRepository = new(Fixture.CreateContext());
    }

    private JustLabel.Data.AppDbContext Initialize()
    {
        var context = Fixture.CreateContext();

        var user1 = new UserDbModelBuilder()
            .WithId(1)
            .Build();

        context.Users.Add(user1);

        var dataset1 = new DatasetDbModelBuilder()
            .WithId(5)
            .WithCreatorId(1)
            .Build();

        var dataset2 = new DatasetDbModelBuilder()
            .WithId(6)
            .WithCreatorId(1)
            .Build();

        context.Datasets.Add(dataset1);
        context.Datasets.Add(dataset2);

        context.SaveChanges();

        return context;
    }

    [Fact]
    public void TestAddImageInEmptyTable()
    {
        using var context = Initialize();

        // Arrange
        var image = ImageModelFactory.Create(
            1,
            5,
            "path/to/image.jpg",
            1920,
            1080
        );

        // Act
        _imageRepository.Add(image);

        // Assert
        var images = (from i in context.Images select i).ToList();
        Assert.Single(images);
        Assert.Equal(image.DatasetId, images[0].DatasetId);
        Assert.Equal(image.Path, images[0].Path);
        Assert.Equal(image.Width, images[0].Width);
        Assert.Equal(image.Height, images[0].Height);
    }

    [Fact]
    public void TestAddImageInNonEmptyTable()
    {
        using var context = Initialize();

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

        context.Images.Add(image1);
        context.SaveChanges();

        // Act
        _imageRepository.Add(image2);

        // Assert
        var images = (from i in context.Images select i).ToList();
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
        using var context = Initialize();

        // Arrange
        var imageDbo = ImageDbModelFactory.Create(1, 5, "path/to/image.jpg", 1920, 1080);

        context.Images.Add(imageDbo);
        context.SaveChanges();

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
        using var context = Initialize();

        // Arrange

        // Act
        var resultImage = _imageRepository.Get(1);

        // Assert
        Assert.Null(resultImage);
    }

    [Fact]
    public void TestGetAllImagesForDataset()
    {
        using var context = Initialize();

        // Arrange
        var imageDbo1 = ImageDbModelFactory.Create(1, 5, "path/to/image1.jpg", 1920, 1080);
        var imageDbo2 = ImageDbModelFactory.Create(2, 5, "path/to/image2.jpg", 1280, 720);
        var imageDbo3 = ImageDbModelFactory.Create(3, 6, "path/to/image2.jpg", 1280, 720);

        context.Images.Add(imageDbo1);
        context.Images.Add(imageDbo2);
        context.Images.Add(imageDbo3);
        context.SaveChanges();

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
        using var context = Initialize();

        // Arrange

        // Act
        var resultImages = _imageRepository.GetAll(5);

        // Assert
        Assert.Empty(resultImages);
    }
}
