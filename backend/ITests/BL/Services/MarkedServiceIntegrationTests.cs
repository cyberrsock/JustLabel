using Xunit;
using Moq;
using JustLabel.Data;
using JustLabel.Models;
using JustLabel.Exceptions;
using JustLabel.Repositories;
using JustLabel.Services;
using IntegrationTests.Data;
using IntegrationTests.Builders;
using IntegrationTests.Factories;

namespace IntegrationTests.Services;

[Collection("Test Database")]
public class MarkedServiceIntegrationTests : BaseServiceIntegrationTests
{
    private readonly AppDbContext _context;
    private readonly MarkedService _markedService;
    private readonly SchemeRepository _schemeRepository;
    private readonly MarkedRepository _markedRepository;
    private readonly UserRepository _userRepository;
    private readonly ImageRepository _imageRepository;

    public MarkedServiceIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _context = Fixture.CreateContext();
        _schemeRepository = new SchemeRepository(_context);
        _markedRepository = new MarkedRepository(_context);
        _userRepository = new UserRepository(_context);
        _imageRepository = new ImageRepository(_context);
        _markedService = new MarkedService(
            _schemeRepository,
            _markedRepository,
            _userRepository,
            _imageRepository
        );
    }

    private JustLabel.Data.AppDbContext Initialize()
    {
        var context = Fixture.CreateContext();

        var user1 = new UserDbModelBuilder()
            .WithId(1)
            .Build();

        context.Users.Add(user1);

        var user2 = new UserDbModelBuilder()
            .WithId(2)
            .WithIsAdmin(true)
            .Build();

        context.Users.Add(user2);

        var dataset1 = new DatasetDbModelBuilder()
            .WithId(1)
            .WithCreatorId(1)
            .Build();
        
        context.Datasets.Add(dataset1);

        var image1 = new ImageDbModelBuilder()
            .WithId(1)
            .WithDatasetId(1)
            .Build();
        
        context.Images.Add(image1);

        var label1 = new LabelDbModelBuilder()
            .WithId(1)
            .Build();
        
        context.Labels.Add(label1);

        var scheme1 = new SchemeDbModelBuilder()
            .WithId(1)
            .WithCreatorId(1)
            .Build();
        
        context.Schemes.Add(scheme1);

        context.SaveChanges();

        return context;
    }

    [Fact]
    public void Create_ShouldSucceed_WhenValidModel()
    {
        using var context = Initialize();

        // Arrange
        var markedModel = new MarkedModelBuilder()
            .WithId(1)
            .WithImageId(1)
            .WithSchemeId(1)
            .WithCreatorId(1)
            .WithAreaModels([])
            .Build();
        
        // Act
        _markedService.Create(markedModel);
        
        // Assert
        var marks = (from m in context.Marked select m).ToList();
        Assert.Single(marks);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenCreatorDoesNotExist()
    {
        using var context = Initialize();

        // Arrange
        var markedModel = new MarkedModelBuilder()
            .WithId(1)
            .WithImageId(1)
            .WithSchemeId(1)
            .WithCreatorId(3)
            .WithAreaModels([])
            .Build();

        // Act
        var exception = Assert.Throws<MarkedException>(() => _markedService.Create(markedModel));
        
        // Assert
        Assert.Equal("CreatorId does not exist in the users list", exception.Message);
    }

    [Fact]
    public void Delete_ShouldSucceed_WhenValidId()
    {
        using var context = Initialize();

        // Arrange
        var markedModel = new MarkedDbModelBuilder()
            .WithImageId(1)
            .WithSchemeId(1)
            .WithCreatorId(1)
            .Build();
        context.Marked.Add(markedModel);
        context.SaveChanges();

        // Act
        _markedService.Delete(1);

        // Assert
        var marks = (from m in context.Marked select m).ToList();
        Assert.Empty(marks);
    }

    [Fact]
    public void Get_ShouldReturnMarkedModel_WhenValidModel()
    {
        using var context = Initialize();

        // Arrange
        var markedModel = new MarkedModelBuilder().WithId(1).WithAreaModels([]).WithCreatorId(1).WithImageId(1).WithSchemeId(1).Build();
        context.Marked.Add(MarkedDbModelFactory.Create(markedModel));
        context.SaveChanges();

        // Act
        var result = _markedService.Get(markedModel);

        // Assert
        Assert.Equal(markedModel.Id, result.Id);
        Assert.Equal(markedModel.CreatorId, result.CreatorId);
        Assert.Equal(markedModel.ImageId, result.ImageId);
        Assert.Equal(markedModel.SchemeId, result.SchemeId);
    }

    [Fact]
    public void Get_ShouldThrowException_WhenMarkedModelNotFound()
    {
        using var context = Initialize();

        // Arrange
        var markedModel = new MarkedModelBuilder().WithId(1).WithCreatorId(1).WithSchemeId(1).WithImageId(1).Build();

        // Act
        var exception = Assert.Throws<MarkedException>(() => _markedService.Get(markedModel));
        
        // Assert
        Assert.Equal("Marked not found", exception.Message);
    }

    [Fact]
    public void GetAll_ShouldReturnAllMarkedModels_WhenAdminIdIsAdmin()
    {
        using var context = Initialize();

        // Arrange
        var adminId = 1;
        var markedModels = new List<MarkedModel>
        {
            new MarkedModelBuilder().WithId(1).WithAreaModels([]).WithCreatorId(1).WithImageId(1).WithSchemeId(1).WithIsBlocked(false).Build(),
            new MarkedModelBuilder().WithId(2).WithAreaModels([]).WithCreatorId(1).WithImageId(1).WithSchemeId(1).WithIsBlocked(false).Build(),
        };

        context.Marked.Add(MarkedDbModelFactory.Create(markedModels[0]));
        context.Marked.Add(MarkedDbModelFactory.Create(markedModels[1]));
        context.SaveChanges();

        // Act
        var result = _markedService.GetAll(adminId);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetAll_ShouldReturnNonBlockedMarkedModels_WhenAdminIdIsNotAdmin()
    {
        using var context = Initialize();

        // Arrange
        var adminId = 1;
        var markedModels = new List<MarkedModel>
        {
            new MarkedModelBuilder().WithId(1).WithCreatorId(1).WithImageId(1).WithSchemeId(1).WithIsBlocked(false).Build(),
            new MarkedModelBuilder().WithId(2).WithCreatorId(1).WithImageId(1).WithSchemeId(1).WithIsBlocked(true).Build(),
            new MarkedModelBuilder().WithId(3).WithCreatorId(1).WithImageId(1).WithSchemeId(1).WithIsBlocked(false).Build(),
        };

        context.Marked.Add(MarkedDbModelFactory.Create(markedModels[0]));
        context.Marked.Add(MarkedDbModelFactory.Create(markedModels[1]));
        context.Marked.Add(MarkedDbModelFactory.Create(markedModels[2]));
        context.SaveChanges();

        // Act
        var result = _markedService.GetAll(adminId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, m => Assert.False(m.IsBlocked));
    }

    [Fact]
    public void Get_ShouldReturnMarkedModels_WhenAdminIdIsAdmin()
    {
        using var context = Initialize();

        // Arrange
        var datasetId = 1;
        var adminId = 2;
        var markedModels = new List<MarkedModel>
        {
            new MarkedModelBuilder().WithId(1).WithAreaModels([]).WithCreatorId(1).WithImageId(1).WithSchemeId(1).WithCreatorId(adminId).WithIsBlocked(true).Build(),
            new MarkedModelBuilder().WithId(2).WithAreaModels([]).WithCreatorId(1).WithImageId(1).WithSchemeId(1).WithIsBlocked(false).Build(),
        };

        context.Marked.Add(MarkedDbModelFactory.Create(markedModels[0]));
        context.Marked.Add(MarkedDbModelFactory.Create(markedModels[1]));
        context.SaveChanges();
        
        // Act
        var result = _markedService.Get(datasetId, adminId);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Get_ShouldReturnFilteredMarkedModels_WhenAdminIdIsNotAdmin()
    {
        using var context = Initialize();

        // Arrange
        var datasetId = 1;
        var adminId = 2;
        var markedModels = new List<MarkedModel>
        {
            new MarkedModelBuilder().WithId(1).WithCreatorId(1).WithImageId(1).WithSchemeId(1).WithIsBlocked(true).WithCreatorId(adminId).Build(),
            new MarkedModelBuilder().WithId(2).WithCreatorId(1).WithImageId(1).WithSchemeId(1).WithIsBlocked(true).Build(),
            new MarkedModelBuilder().WithId(3).WithCreatorId(1).WithImageId(1).WithSchemeId(1).WithIsBlocked(false).Build()
        };

        context.Marked.Add(MarkedDbModelFactory.Create(markedModels[0]));
        context.Marked.Add(MarkedDbModelFactory.Create(markedModels[1]));
        context.Marked.Add(MarkedDbModelFactory.Create(markedModels[2]));
        context.SaveChanges();
        
        // Act
        var result = _markedService.Get(datasetId, adminId);

        // Assert
        Assert.Equal(1, result.First().Id);
    }
}
