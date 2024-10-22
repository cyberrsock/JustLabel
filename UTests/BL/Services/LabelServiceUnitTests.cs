using Xunit;
using Moq;
using JustLabel.Models;
using JustLabel.Exceptions;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services;
using UnitTests.Builders;

namespace UnitTests.Services;

public class LabelServiceUnitTests
{
    private readonly LabelService _labelService;
    private readonly Mock<ILabelRepository> _mockLabelRepository = new();
    private readonly Mock<ISchemeRepository> _mockSchemeRepository = new();

    public LabelServiceUnitTests()
    {
        _labelService = new LabelService(
            _mockLabelRepository.Object,
            _mockSchemeRepository.Object
        );
    }

    [Fact]
    public void TestAddNewLabel()
    {
        // Arrange
        var label = new LabelModelBuilder()
            .WithTitle("test label")
            .Build();
        
        List<LabelModel> existingLabels = new List<LabelModel>();

        _mockLabelRepository.Setup(s => s.Get()).Returns(existingLabels);
        _mockLabelRepository.Setup(s => s.Add(It.IsAny<LabelModel>())).Returns(1);

        // Act
        int labelId = _labelService.Add(label);

        // Assert
        Assert.Equal(1, labelId);
        Assert.Equal("Test label", label.Title);
        _mockLabelRepository.Verify(s => s.Add(It.IsAny<LabelModel>()), Times.Once);
    }

    [Fact]
    public void TestAddExistingLabel()
    {
        // Arrange
        var existingLabel = new LabelModelBuilder()
            .WithId(1)
            .WithTitle("Test Label")
            .Build();
        
        var newLabel = new LabelModelBuilder()
            .WithId(2)
            .WithTitle("test label")
            .Build();

        List<LabelModel> existingLabels = [existingLabel];

        _mockLabelRepository.Setup(s => s.Get()).Returns(existingLabels);
        _mockLabelRepository.Setup(s => s.Add(It.IsAny<LabelModel>())).Returns(newLabel.Id);

        // Act
        int labelId = _labelService.Add(newLabel);

        // Assert
        Assert.Equal(2, labelId);
    }

    [Fact]
    public void TestDeleteExistingLabel()
    {
        // Arrange
        int labelId = 1;
        var label = new LabelModelBuilder().WithId(labelId).WithTitle("Test Label").Build();
        var schemes = new List<SchemeModel>();

        _mockLabelRepository.Setup(s => s.Get()).Returns([label]);
        _mockSchemeRepository.Setup(s => s.GetAll()).Returns(schemes);

        // Act
        _labelService.Delete(labelId);

        // Assert
        _mockLabelRepository.Verify(s => s.Delete(labelId), Times.Once);
    }

    [Fact]
    public void TestDeleteLabelThatDoesNotExist()
    {
        // Arrange
        int labelId = 1;

        _mockLabelRepository.Setup(s => s.Get()).Returns([]);

        // Act
        var exception = Assert.Throws<LabelException>(() => _labelService.Delete(labelId));

        // Assert
        Assert.Equal("The label does not exist", exception.Message);
        _mockLabelRepository.Verify(s => s.Delete(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void TestDeleteLabelAssociatedWithScheme()
    {
        // Arrange
        int labelId = 1;
        var label = new LabelModelBuilder().WithId(labelId).Build();
        var scheme = new SchemeModelBuilder()
            .WithId(1)
            .WithLabelIds([label])
            .Build();

        _mockLabelRepository.Setup(s => s.Get()).Returns([label]);
        _mockSchemeRepository.Setup(s => s.GetAll()).Returns([scheme]);

        // Act
        var exception = Assert.Throws<LabelException>(() => _labelService.Delete(labelId));

        // Assert
        Assert.Equal("The label is already associated with a scheme", exception.Message);
        _mockLabelRepository.Verify(s => s.Delete(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void TestGetAllLabels()
    {
        // Arrange
        var label1 = new LabelModelBuilder().WithId(1).WithTitle("Label 1").Build();
        var label2 = new LabelModelBuilder().WithId(2).WithTitle("Label 2").Build();
        var labels = new List<LabelModel> { label1, label2 };

        _mockLabelRepository.Setup(s => s.Get()).Returns(labels);

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
        // Arrange
        _mockLabelRepository.Setup(s => s.Get()).Returns([]);

        // Act
        var result = _labelService.Get();

        // Assert
        Assert.Empty(result);
    }
}

