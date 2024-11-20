using Xunit;
using Moq;
using JustLabel.Models;
using JustLabel.Exceptions;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services;
using UnitTests.Builders;

namespace UnitTests.Services;

public class MarkedServiceTests
{
    private readonly Mock<ISchemeRepository> _mockSchemeRepository;
    private readonly Mock<IMarkedRepository> _mockMarkedRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IImageRepository> _mockImageRepository;
    private readonly MarkedService _markedService;

    public MarkedServiceTests()
    {
        _mockSchemeRepository = new Mock<ISchemeRepository>();
        _mockMarkedRepository = new Mock<IMarkedRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockImageRepository = new Mock<IImageRepository>();
        _markedService = new MarkedService(
            _mockSchemeRepository.Object,
            _mockMarkedRepository.Object,
            _mockUserRepository.Object,
            _mockImageRepository.Object
        );
    }

    [Fact]
    public void Create_ShouldSucceed_WhenValidModel()
    {
        // Arrange
        var markedModel = new MarkedModelBuilder()
            .WithImageId(1)
            .WithSchemeId(1)
            .WithCreatorId(1)
            .Build();

        _mockUserRepository.Setup(u => u.GetUserById(1)).Returns(new UserModel());
        _mockSchemeRepository.Setup(s => s.Get(1)).Returns(new SchemeModel());
        _mockImageRepository.Setup(i => i.Get(1)).Returns(new ImageModel());

        // Act
        _markedService.Create(markedModel);

        // Assert
        _mockMarkedRepository.Verify(m => m.Create(markedModel), Times.Once);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenCreatorDoesNotExist()
    {
        // Arrange
        var markedModel = new MarkedModelBuilder()
            .WithImageId(1)
            .WithSchemeId(1)
            .WithCreatorId(1)
            .Build();

        _mockUserRepository.Setup(u => u.GetUserById(1)).Returns((UserModel)null);

        // Act
        var exception = Assert.Throws<MarkedException>(() => _markedService.Create(markedModel));

        // Assert
        Assert.Equal("CreatorId does not exist in the users list", exception.Message);
    }

    [Fact]
    public void Delete_ShouldSucceed_WhenValidId()
    {
        // Arrange
        var id = 1;

        // Act
        _markedService.Delete(id);

        // Assert
        _mockMarkedRepository.Verify(m => m.Delete(id), Times.Once);
    }

    [Fact]
    public void Delete_ShouldLogAndHandleException_WhenIdDoesNotExist()
    {
        // Arrange
        var id = 1;

        _mockMarkedRepository.Setup(m => m.Delete(id)).Throws(new MarkedException("Not found"));

        // Act
        var exception = Assert.Throws<MarkedException>(() => _markedService.Delete(id));

        // Assert
        Assert.Equal("Not found", exception.Message);
    }

    [Fact]
    public void Get_ShouldReturnMarkedModel_WhenValidModel()
    {
        // Arrange
        var markedModel = new MarkedModelBuilder().WithId(1).Build();
        _mockMarkedRepository.Setup(m => m.Get(markedModel)).Returns(markedModel);

        // Act
        var result = _markedService.Get(markedModel);

        // Assert
        Assert.Equal(markedModel, result);
    }

    [Fact]
    public void Get_ShouldThrowException_WhenMarkedModelNotFound()
    {
        // Arrange
        var markedModel = new MarkedModelBuilder().WithId(1).Build();
        _mockMarkedRepository.Setup(m => m.Get(markedModel)).Returns((MarkedModel)null);

        // Act
        var exception = Assert.Throws<MarkedException>(() => _markedService.Get(markedModel));

        // Assert
        Assert.Equal("Marked not found", exception.Message);
    }

    [Fact]
    public void GetAll_ShouldReturnAllMarkedModels_WhenAdminIdIsAdmin()
    {
        // Arrange
        var adminId = 1;
        var markedModels = new List<MarkedModel>
        {
            new MarkedModelBuilder().WithId(1).WithIsBlocked(false).Build(),
            new MarkedModelBuilder().WithId(2).WithIsBlocked(false).Build(),
        };

        _mockMarkedRepository.Setup(m => m.GetAll()).Returns(markedModels);
        _mockUserRepository.Setup(u => u.GetUserById(adminId)).Returns(new UserModel { IsAdmin = true });

        // Act
        var result = _markedService.GetAll(adminId);

        // Assert
        Assert.Equal(markedModels, result);
    }

    [Fact]
    public void GetAll_ShouldReturnNonBlockedMarkedModels_WhenAdminIdIsNotAdmin()
    {
        // Arrange
        var adminId = 2;
        var markedModels = new List<MarkedModel>
        {
            new MarkedModelBuilder().WithId(1).WithIsBlocked(false).Build(),
            new MarkedModelBuilder().WithId(2).WithIsBlocked(true).Build(),
            new MarkedModelBuilder().WithId(3).WithIsBlocked(false).Build(),
        };

        _mockMarkedRepository.Setup(m => m.GetAll()).Returns(markedModels);
        _mockUserRepository.Setup(u => u.GetUserById(adminId)).Returns(new UserModel { IsAdmin = false });

        // Act
        var result = _markedService.GetAll(adminId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, m => Assert.False(m.IsBlocked));
    }

    [Fact]
    public void Get_ShouldReturnMarkedModels_WhenAdminIdIsAdmin()
    {
        // Arrange
        var datasetId = 1;
        var adminId = 1;
        var markedModels = new List<MarkedModel>
        {
            new MarkedModelBuilder().WithId(1).WithCreatorId(adminId).WithIsBlocked(true).Build(),
            new MarkedModelBuilder().WithId(2).WithIsBlocked(false).Build(),
        };

        _mockMarkedRepository.Setup(m => m.Get_By_DatasetId(datasetId)).Returns(markedModels);
        _mockUserRepository.Setup(u => u.GetUserById(adminId)).Returns(new UserModel { IsAdmin = true });

        // Act
        var result = _markedService.Get(datasetId, adminId);

        // Assert
        Assert.Equal(markedModels, result);
    }

    [Fact]
    public void Get_ShouldReturnFilteredMarkedModels_WhenAdminIdIsNotAdmin()
    {
        // Arrange
        var datasetId = 1;
        var adminId = 2;
        var markedModels = new List<MarkedModel>
        {
            new MarkedModelBuilder().WithId(1).WithIsBlocked(true).WithCreatorId(adminId).Build(),
            new MarkedModelBuilder().WithId(2).WithIsBlocked(true).Build(),
            new MarkedModelBuilder().WithId(3).WithIsBlocked(false).Build()
        };

        _mockMarkedRepository.Setup(m => m.Get_By_DatasetId(datasetId)).Returns(markedModels);
        _mockUserRepository.Setup(u => u.GetUserById(adminId)).Returns(new UserModel { IsAdmin = false });

        // Act
        var result = _markedService.Get(datasetId, adminId);

        // Assert
        Assert.Equal(1, result.First().Id);
    }

    [Fact]
    public void Update_Rects_ShouldUpdateMarkedModel_WhenCalledWithValidModel()
    {
        // Arrange
        var markedModel = new MarkedModelBuilder().WithId(1).Build();

        // Act
        _markedService.Update_Rects(markedModel);

        // Assert
        _mockMarkedRepository.Verify(m => m.Update_Rects(markedModel), Times.Once);
    }

    [Fact]
    public void Update_Rects_ShouldThrowException_WhenUpdateFails()
    {
        // Arrange
        var markedModel = new MarkedModelBuilder().WithId(1).Build();
        _mockMarkedRepository.Setup(m => m.Update_Rects(markedModel)).Throws(new Exception("Update Failed"));

        // Act
        var exception = Assert.Throws<Exception>(() => _markedService.Update_Rects(markedModel));

        // Assert
        Assert.Equal("Update Failed", exception.Message);
    }

    [Fact]
    public void Update_Block_ShouldUpdateMarkedModel_WhenUserIsAdmin()
    {
        // Arrange
        var markedModel = new MarkedModelBuilder().WithId(1).WithCreatorId(1).Build();
        _mockUserRepository.Setup(u => u.GetUserById(1)).Returns(new UserModel { IsAdmin = true });

        // Act
        _markedService.Update_Block(markedModel);

        // Assert
        _mockMarkedRepository.Verify(m => m.Update_Block(markedModel), Times.Once);
    }

    [Fact]
    public void Update_Block_ShouldThrowException_WhenUserIsNotAdmin()
    {
        // Arrange
        var markedModel = new MarkedModelBuilder().WithId(1).WithCreatorId(1).Build();
        _mockUserRepository.Setup(u => u.GetUserById(1)).Returns(new UserModel { IsAdmin = false });

        // Act
        var exception = Assert.Throws<AdminUserException>(() => _markedService.Update_Block(markedModel));

        // Assert
        Assert.Equal("The user with AdminId is not admin", exception.Message);
    }


}
