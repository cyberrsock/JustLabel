using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using IntegrationTests.Data;
using IntegrationTests.Factories;

namespace IntegrationTests.Repositories;

[Collection("Test Database")]
public class LabelRepositoryIntegrationTests : BaseRepositoryIntegrationTests
{
    private readonly LabelRepository _labelRepository;

    public LabelRepositoryIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _labelRepository = new (Fixture.CreateContext());
    }

    private JustLabel.Data.AppDbContext Initialize()
    {
        var context = Fixture.CreateContext();
        return context;
    }

    [Fact]
    public void TestAddLabelInEmptyTable()
    {
        using var context = Initialize();

        // Arrange
        var label = LabelModelFactory.Create(
            1,
            "Test Label"
        );

        // Act
        int labelId = _labelRepository.Add(label);

        // Assert
        var labels = (from l in context.Labels select l).ToList();
        Assert.Single(labels);
        Assert.Equal(label.Id, labelId);
        Assert.Equal(label.Id, labels[0].Id);
        Assert.Equal(label.Title, labels[0].Title);
    }

    [Fact]
    public void TestAddLabelInNonEmptyTable()
    {
        using var context = Initialize();

        // Arrange
        var label1 = LabelDbModelFactory.Create(
            1,
            "Test Label"
        );

        var label2 = LabelModelFactory.Create(
            2,
            "Test Label2"
        );

        context.Labels.Add(label1);
        context.SaveChanges();

        // Act
        int labelId2 = _labelRepository.Add(label2);

        // Assert
        var labels = (from l in context.Labels select l).ToList();
        Assert.Equal(2, labels.Count);
        Assert.Equal(label2.Id, labelId2);
        Assert.Equal(label2.Id, labels[1].Id);
        Assert.Equal(label2.Title, labels[1].Title);
    }

    [Fact]
    public void TestDeleteExistingLabel()
    {
        using var context = Initialize();

        // Arrange
        context.Labels.Add(LabelDbModelFactory.Create(1, "Label to Delete"));
        context.SaveChanges();

        // Act
        _labelRepository.Delete(1);

        // Assert
        var labels = (from l in context.Labels select l).ToList();
        Assert.Empty(labels);
    }

    [Fact]
    public void TestDeleteNonExistentLabel()
    {
        using var context = Initialize();

        // Arrange
        context.Labels.Add(LabelDbModelFactory.Create(1, "Label to Delete"));
        context.SaveChanges();

        // Act
        _labelRepository.Delete(2);

        // Assert
        var labels = (from l in context.Labels select l).ToList();
        Assert.Single(labels);
    }

    [Fact]
    public void TestGetAllLabels()
    {
        using var context = Initialize();

        // Arrange
        var labelDbo1 = LabelDbModelFactory.Create(1, "Label 1");
        var labelDbo2 = LabelDbModelFactory.Create(2, "Label 2");

        context.Labels.Add(labelDbo1);
        context.Labels.Add(labelDbo2);
        context.SaveChanges();

        // Act
        var resultLabels = _labelRepository.Get();

        // Assert
        Assert.Equal(2, resultLabels.Count);
        Assert.Equal(labelDbo1.Id, resultLabels[0].Id);
        Assert.Equal(labelDbo1.Title, resultLabels[0].Title);
        Assert.Equal(labelDbo2.Id, resultLabels[1].Id);
        Assert.Equal(labelDbo2.Title, resultLabels[1].Title);
    }

    [Fact]
    public void TestGetNoLabels()
    {
        using var context = Initialize();

        // Arrange

        // Act
        var resultLabels = _labelRepository.Get();

        // Assert
        Assert.Empty(resultLabels);
    }
}

