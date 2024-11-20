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
public class LabelServiceIntegrationTests : BaseServiceIntegrationTests
{
    private readonly AppDbContext _context;
    private readonly LabelService _labelService;
    private readonly LabelRepository _labelRepository;
    private readonly SchemeRepository _schemeRepository;

    public LabelServiceIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _context = Fixture.CreateContext();
        _labelRepository = new LabelRepository(_context);
        _schemeRepository = new SchemeRepository(_context);
        _labelService = new LabelService(_labelRepository, _schemeRepository);
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
    public void TestAddNewLabel()
    {
        using var context = Initialize();

        // Arrange
        var label = new LabelModelBuilder()
            .WithTitle("test label")
            .Build();

        // Act
        int labelId = _labelService.Add(label);

        // Assert
        Assert.Equal(1, labelId);
        Assert.Equal("Test label", label.Title);
    }

    [Fact]
    public void TestAddExistingLabel()
    {
        using var context = Initialize();

        // Arrange
        var existingLabel = new LabelDbModelBuilder()
            .WithId(1)
            .WithTitle("Test Label")
            .Build();

        var newLabel = new LabelModelBuilder()
            .WithId(2)
            .WithTitle("test label")
            .Build();

        context.Labels.Add(existingLabel);
        context.SaveChanges();

        // Act
        int labelId = _labelService.Add(newLabel);

        // Assert
        Assert.Equal(2, labelId);
    }

    [Fact]
    public void TestDeleteExistingLabel()
    {
        using var context = Initialize();

        // Arrange
        int labelId = 1;
        var label = new LabelDbModelBuilder().WithId(labelId).WithTitle("Test Label").Build();
        context.Labels.Add(label);
        context.SaveChanges();

        // Act
        _labelService.Delete(labelId);

        // Assert
        var labels = (from l in context.Labels select l).ToList();
        Assert.Empty(labels);
    }

    [Fact]
    public void TestDeleteLabelThatDoesNotExist()
    {
        using var context = Initialize();

        // Arrange
        int labelId = 1;

        // Act
        var exception = Assert.Throws<LabelException>(() => _labelService.Delete(labelId));

        // Assert
        Assert.Equal("The label does not exist", exception.Message);
    }

    [Fact]
    public void TestDeleteLabelAssociatedWithScheme()
    {
        using var context = Initialize();

        // Arrange
        int labelId = 1;
        var label = new LabelDbModelBuilder().WithId(labelId).Build();
        var scheme = new SchemeDbModelBuilder()
            .WithId(1)
            .WithCreatorId(1)
            .Build();
        context.Labels.Add(label);
        context.Schemes.Add(scheme);
        context.LabelsSchemes.Add(LabelSchemeDbModelFactory.Create(1, 1));
        context.SaveChanges();

        // Act
        var exception = Assert.Throws<LabelException>(() => _labelService.Delete(labelId));

        // Assert
        Assert.Equal("The label is already associated with a scheme", exception.Message);
    }

    [Fact]
    public void TestGetAllLabels()
    {
        using var context = Initialize();

        // Arrange
        var label1 = new LabelDbModelBuilder().WithId(1).WithTitle("Label 1").Build();
        var label2 = new LabelDbModelBuilder().WithId(2).WithTitle("Label 2").Build();
        context.Labels.Add(label1);
        context.Labels.Add(label2);
        context.SaveChanges();

        // Act
        var result = _labelService.Get();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, l => l.Title == "Label 1");
        Assert.Contains(result, l => l.Title == "Label 2");
    }

    [Fact]
    public void TestGetAllLabelsWhenNoLabelsExist()
    {
        using var context = Initialize();

        // Arrange

        // Act
        var result = _labelService.Get();

        // Assert
        Assert.Empty(result);
    }
}

