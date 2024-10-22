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
public class SchemeServiceIntegrationTests : BaseServiceIntegrationTests
{
    private readonly AppDbContext _context;
    private readonly SchemeService _schemeService;
    private readonly SchemeRepository _schemeRepository;
    private readonly UserRepository _userRepository;
    private readonly MarkedRepository _markedRepository;

    public SchemeServiceIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _context = Fixture.CreateContext();
        _schemeRepository = new SchemeRepository(_context);
        _userRepository = new UserRepository(_context);
        _markedRepository = new MarkedRepository(_context);
        _schemeService = new SchemeService(
            _schemeRepository,
            _userRepository,
            _markedRepository
        );
    }

    private JustLabel.Data.AppDbContext Initialize()
    {
        var context = Fixture.CreateContext();

        var user1 = new UserDbModelBuilder()
            .WithId(1)
            .Build();

        context.Users.Add(user1);

        context.SaveChanges();

        return context;
    }

    [Fact]
    public void TestAddSchemeWithValidData()
    {
        using var context = Initialize();
        
        // Arrange
        var scheme = new SchemeModelBuilder()
            .WithId(1)
            .WithTitle("Valid Scheme")
            .WithCreatorId(1)
            .WithLabelIds(new List<LabelModel> { new LabelModel() { Id = 1 }, new LabelModel() { Id = 2 } })
            .Build();
        
        context.Labels.Add(LabelDbModelFactory.Create(1, "Test1"));
        context.Labels.Add(LabelDbModelFactory.Create(2, "Test2"));
        context.SaveChanges();

        // Act
        _schemeService.Add(scheme);

        // Assert
        var schemes = (from s in context.Schemes select s).ToList();
        Assert.Single(schemes);
    }

    [Fact]
    public void TestAddSchemeWithEmptyTitle()
    {
        using var context = Initialize();

        // Arrange
        var scheme = new SchemeModelBuilder()
            .WithId(1)
            .WithTitle("")
            .WithCreatorId(1)
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
        using var context = Initialize();

        // Arrange
        int schemeId = 1;
        var scheme = new SchemeDbModelBuilder().WithId(schemeId).WithCreatorId(1).Build();
        context.Schemes.Add(scheme);
        context.SaveChanges();

        // Act
        _schemeService.Delete(schemeId);

        // Assert
        var schemes = (from s in context.Schemes select s).ToList();
        Assert.Empty(schemes);
    }

    [Fact]
    public void TestDeleteSchemeWithNonExistingId()
    {
        using var context = Initialize();

        // Arrange
        int schemeId = 1;

        // Act
        var exception = Assert.Throws<SchemeException>(() => _schemeService.Delete(schemeId));

        // Assert
        Assert.Equal("Scheme with this id does not exist", exception.Message);
    }

    [Fact]
    public void TestGetSchemeWithExistingId()
    {
        using var context = Initialize();

        // Arrange
        int schemeId = 1;
        var scheme = new SchemeDbModelBuilder().WithId(schemeId).WithCreatorId(1).WithTitle("Existing Scheme").Build();
        context.Schemes.Add(scheme);
        context.SaveChanges();

        // Act
        var result = _schemeService.Get(schemeId);

        // Assert
        Assert.Equal(schemeId, result.Id);
        Assert.Equal("Existing Scheme", result.Title);
    }

    [Fact]
    public void TestGetSchemeWithNonExistingId()
    {
        using var context = Initialize();
        
        // Arrange
        int schemeId = 1;

        // Act
        var exception = Assert.Throws<SchemeException>(() => _schemeService.Get(schemeId));

        // Assert
        Assert.Equal("Scheme with this id does not exist", exception.Message);
    }

    [Fact]
    public void TestGetAllSchemes()
    {
        using var context = Initialize();

        // Arrange
        var scheme1 = new SchemeDbModelBuilder().WithId(1).WithCreatorId(1).WithTitle("Scheme 1").Build();
        var scheme2 = new SchemeDbModelBuilder().WithId(2).WithCreatorId(1).WithTitle("Scheme 2").Build();
        context.Schemes.Add(scheme1);
        context.Schemes.Add(scheme2);
        context.SaveChanges();

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
        using var context = Initialize();

        // Arrange

        // Act
        var result = _schemeService.Get();

        // Assert
        Assert.Empty(result);
    }
}
