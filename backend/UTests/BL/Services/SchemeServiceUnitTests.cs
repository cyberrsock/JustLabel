using Xunit;
using Moq;
using JustLabel.Models;
using JustLabel.Exceptions;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services;
using UnitTests.Builders;

namespace UnitTests.Services;

public class SchemeServiceUnitTests
{
    private readonly SchemeService _schemeService;
    private readonly Mock<ISchemeRepository> _mockSchemeRepository = new();
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly Mock<IMarkedRepository> _mockMarkedRepository = new();

    public SchemeServiceUnitTests()
    {
        _schemeService = new SchemeService(
            _mockSchemeRepository.Object,
            _mockUserRepository.Object,
            _mockMarkedRepository.Object
        );
    }

    [Fact]
    public void TestAddSchemeWithValidData()
    {
        // Arrange
        var scheme = new SchemeModelBuilder()
            .WithId(1)
            .WithTitle("Valid Scheme")
            .WithCreatorId(2)
            .WithLabelIds(new List<LabelModel> { new LabelModel() { Id = 1 }, new LabelModel() { Id = 2 } })
            .Build();

        _mockUserRepository
            .Setup(s => s.GetUserById(scheme.CreatorId))
            .Returns(new UserModelBuilder().WithId(2).Build());

        // Act
        _schemeService.Add(scheme);

        // Assert
        _mockSchemeRepository.Verify(s => s.Add(scheme), Times.Once);
    }

    [Fact]
    public void TestAddSchemeWithEmptyTitle()
    {
        // Arrange
        var scheme = new SchemeModelBuilder()
            .WithId(1)
            .WithTitle("")
            .WithCreatorId(2)
            .WithLabelIds(new List<LabelModel> { new LabelModel() { Id = 1 } })
            .Build();

        // Act
        var exception = Assert.Throws<SchemeException>(() => _schemeService.Add(scheme));

        // Assert
        Assert.Equal("Title field cannot be empty", exception.Message);
    }

    [Fact]
    public void TestDeleteSchemeSuccessfully()
    {
        // Arrange
        int schemeId = 1;
        var scheme = new SchemeModelBuilder().WithId(schemeId).Build();

        _mockSchemeRepository.Setup(s => s.Get(schemeId)).Returns(scheme);
        _mockMarkedRepository.Setup(s => s.Get_By_SchemeId(schemeId)).Returns(new List<MarkedModel>());

        // Act
        _schemeService.Delete(schemeId);

        // Assert
        _mockSchemeRepository.Verify(s => s.Delete(schemeId), Times.Once);
    }

    [Fact]
    public void TestDeleteSchemeWithNonExistingId()
    {
        // Arrange
        int schemeId = 1;

        _mockSchemeRepository.Setup(s => s.Get(schemeId)).Returns((SchemeModel)null);

        // Act
        var exception = Assert.Throws<SchemeException>(() => _schemeService.Delete(schemeId));

        // Assert
        Assert.Equal("Scheme with this id does not exist", exception.Message);
    }

    [Fact]
    public void TestGetSchemeWithExistingId()
    {
        // Arrange
        int schemeId = 1;
        var scheme = new SchemeModelBuilder().WithId(schemeId).WithTitle("Existing Scheme").Build();

        _mockSchemeRepository.Setup(s => s.Get(schemeId)).Returns(scheme);

        // Act
        var result = _schemeService.Get(schemeId);

        // Assert
        Assert.Equal(schemeId, result.Id);
        Assert.Equal("Existing Scheme", result.Title);
    }

    [Fact]
    public void TestGetSchemeWithNonExistingId()
    {
        // Arrange
        int schemeId = 1;

        _mockSchemeRepository.Setup(s => s.Get(schemeId)).Returns((SchemeModel)null);

        // Act
        var exception = Assert.Throws<SchemeException>(() => _schemeService.Get(schemeId));

        // Assert
        Assert.Equal("Scheme with this id does not exist", exception.Message);
    }

    [Fact]
    public void TestGetAllSchemes()
    {
        // Arrange
        var schemes = new List<SchemeModel>
        {
            new SchemeModelBuilder().WithId(1).WithTitle("Scheme 1").Build(),
            new SchemeModelBuilder().WithId(2).WithTitle("Scheme 2").Build(),
        };

        _mockSchemeRepository.Setup(s => s.GetAll()).Returns(schemes);

        // Act
        var result = _schemeService.Get();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Scheme 1", result[0].Title);
        Assert.Equal("Scheme 2", result[1].Title);
    }

    [Fact]
    public void TestGetAllSchemesWhenNoSchemesAvailable()
    {
        // Arrange
        _mockSchemeRepository.Setup(s => s.GetAll()).Returns(new List<SchemeModel>());

        // Act
        var result = _schemeService.Get();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void TestUpdateSchemeSuccessfully()
    {
        // Arrange
        var scheme = new SchemeModelBuilder()
            .WithId(1)
            .WithTitle("Updated Scheme")
            .WithCreatorId(2)
            .WithLabelIds(new List<LabelModel> { new LabelModel() { Id = 1 } })
            .Build();

        _mockSchemeRepository.Setup(s => s.Get(scheme.Id)).Returns(scheme);
        _mockUserRepository.Setup(s => s.GetUserById(scheme.CreatorId)).Returns(new UserModelBuilder().WithId(2).Build());

        // Act
        _schemeService.Update(scheme);

        // Assert
        _mockSchemeRepository.Verify(s => s.Update(scheme), Times.Once);
    }

    [Fact]
    public void TestUpdateSchemeWithNonExistingId()
    {
        // Arrange
        var scheme = new SchemeModelBuilder().WithId(1).Build();

        _mockSchemeRepository.Setup(s => s.Get(scheme.Id)).Returns((SchemeModel)null);

        // Act
        var exception = Assert.Throws<SchemeException>(() => _schemeService.Update(scheme));

        // Assert
        Assert.Equal("Scheme with this id does not exist", exception.Message);
    }

    [Fact]
    public void TestUpdateSchemeWithEmptyTitle()
    {
        // Arrange
        var scheme = new SchemeModelBuilder()
            .WithId(1)
            .WithTitle("")
            .WithCreatorId(2)
            .WithLabelIds(new List<LabelModel> { new LabelModel() { Id = 1 } })
            .Build();

        // Arrange for Get check
        _mockSchemeRepository.Setup(s => s.Get(scheme.Id)).Returns(scheme);

        // Act
        var exception = Assert.Throws<SchemeException>(() => _schemeService.Update(scheme));

        // Assert
        Assert.Equal("Title field cannot be empty", exception.Message);
    }

    [Fact]
    public void TestUpdateSchemeWithNoLabels()
    {
        // Arrange
        var scheme = new SchemeModelBuilder()
            .WithId(1)
            .WithTitle("Scheme with No Labels")
            .WithCreatorId(2)
            .WithLabelIds(new List<LabelModel>()) // No labels
            .Build();

        // Arrange for Get check
        _mockSchemeRepository.Setup(s => s.Get(scheme.Id)).Returns(scheme);

        // Act
        var exception = Assert.Throws<SchemeException>(() => _schemeService.Update(scheme));

        // Assert
        Assert.Equal("There are no labels in the scheme", exception.Message);
    }
}
