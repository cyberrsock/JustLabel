using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using UnitTests.Data;
using UnitTests.Factories;

namespace UnitTests.Repositories;

public class LabelRepositoryUnitTests
{
    private readonly LabelRepository _labelRepository;
    private readonly MockDbContextFactory _mockFactory;

    public LabelRepositoryUnitTests()
    {
        _mockFactory = new MockDbContextFactory();
        _labelRepository = new LabelRepository(_mockFactory.MockContext.Object);
    }

    [Fact]
    public void TestAddLabelInEmptyTable()
    {
        // Arrange
        var label = LabelModelFactory.Create(
            1,
            "Test Label"
        );

        List<LabelDbModel> labels = [];
        _mockFactory.SetLabelList(labels);

        // Act
        int labelId = _labelRepository.Add(label);

        // Assert
        Assert.Single(labels);
        Assert.Equal(label.Id, labelId);
        Assert.Equal(label.Id, labels[0].Id);
        Assert.Equal(label.Title, labels[0].Title);
    }

    [Fact]
    public void TestAddLabelInNonEmptyTable()
    {
        // Arrange
        var label1 = LabelDbModelFactory.Create(
            1,
            "Test Label"
        );

        var label2 = LabelModelFactory.Create(
            2,
            "Test Label2"
        );

        List<LabelDbModel> labels = [label1];
        _mockFactory.SetLabelList(labels);

        // Act
        int labelId2 = _labelRepository.Add(label2);

        // Assert
        Assert.Equal(2, labels.Count);
        Assert.Equal(label2.Id, labelId2);
        Assert.Equal(label2.Id, labels[1].Id);
        Assert.Equal(label2.Title, labels[1].Title);
    }

    [Fact]
    public void TestDeleteExistingLabel()
    {
        // Arrange
        List<LabelDbModel> labels = [LabelDbModelFactory.Create(1, "Label to Delete")];
        _mockFactory.SetLabelList(labels);

        // Act
        _labelRepository.Delete(1);

        // Assert
        Assert.Empty(labels);
    }

    [Fact]
    public void TestDeleteNonExistentLabel()
    {
        // Arrange
        List<LabelDbModel> labels = [LabelDbModelFactory.Create(1, "Existing Label")];
        _mockFactory.SetLabelList(labels);

        // Act
        _labelRepository.Delete(2);

        // Assert
        Assert.Single(labels);
    }

    [Fact]
    public void TestGetAllLabels()
    {
        // Arrange
        var labelDbo1 = LabelDbModelFactory.Create(1, "Label 1");
        var labelDbo2 = LabelDbModelFactory.Create(2, "Label 2");

        List<LabelDbModel> labels = [labelDbo1, labelDbo2];
        _mockFactory.SetLabelList(labels);

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
        // Arrange
        List<LabelDbModel> labels = [];
        _mockFactory.SetLabelList(labels);

        // Act
        var resultLabels = _labelRepository.Get();

        // Assert
        Assert.Empty(resultLabels);
    }
}

