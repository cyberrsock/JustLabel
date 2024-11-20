using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using UnitTests.Data;
using UnitTests.Factories;

namespace UnitTests.Repositories;

public class SchemeRepositoryUnitTests
{
    private readonly SchemeRepository _schemeRepository;
    private readonly MockDbContextFactory _mockFactory;

    public SchemeRepositoryUnitTests()
    {
        _mockFactory = new MockDbContextFactory();
        _schemeRepository = new SchemeRepository(_mockFactory.MockContext.Object);
    }

    [Fact]
    public void TestAddSchemeWithoutLabels()
    {
        // Arrange
        var scheme = SchemeModelFactory.Create(
            1,
            "Test Scheme",
            "This is a test scheme.",
            3,
            [],
            DateTime.Now
        );

        List<SchemeDbModel> schemes = [];
        List<LabelSchemeDbModel> labelSchemes = [];
        List<LabelDbModel> labels = [
            LabelDbModelFactory.Create(1, "Test1"),
            LabelDbModelFactory.Create(2, "Test2")
        ];
        _mockFactory.SetSchemeList(schemes);
        _mockFactory.SetLabelSchemeList(labelSchemes);
        _mockFactory.SetLabelList(labels);

        // Act
        _schemeRepository.Add(scheme);

        // Assert
        Assert.Single(schemes);
        Assert.Equal(scheme.Id, schemes[0].Id);
        Assert.Equal(scheme.Title, schemes[0].Title);
        Assert.Equal(scheme.Description, schemes[0].Description);
        Assert.Equal(scheme.CreatorId, schemes[0].CreatorId);
        Assert.Empty(labelSchemes);
    }

    [Fact]
    public void TestAddSchemeWithLabels()
    {
        // Arrange
        var scheme = SchemeModelFactory.Create(
            1,
            "Test Scheme with Labels",
            "This is a test scheme.",
            3,
            [
                LabelModelFactory.Create(1, "Test1"),
                LabelModelFactory.Create(2, "Test2")
            ],
            DateTime.Now
        );

        List<SchemeDbModel> schemes = [];
        List<LabelSchemeDbModel> labelSchemes = [];
        List<LabelDbModel> labels = [
            LabelDbModelFactory.Create(1, "Test1"),
            LabelDbModelFactory.Create(2, "Test2")
        ];
        _mockFactory.SetSchemeList(schemes);
        _mockFactory.SetLabelSchemeList(labelSchemes);
        _mockFactory.SetLabelList(labels);

        // Act
        _schemeRepository.Add(scheme);

        // Assert
        Assert.Single(schemes);
        Assert.Equal(scheme.Id, schemes[0].Id);
        Assert.Equal(scheme.Title, schemes[0].Title);
        Assert.Equal(scheme.Description, schemes[0].Description);
        Assert.Equal(scheme.CreatorId, schemes[0].CreatorId);
        Assert.Equal(2, labelSchemes.Count);
    }

    [Fact]
    public void TestDeleteExistingScheme()
    {
        // Arrange
        var scheme = SchemeModelFactory.Create(1, "Scheme to Delete", "Description", 3, [], DateTime.Now);

        List<SchemeDbModel> schemes = [SchemeDbModelFactory.Create(scheme)];
        _mockFactory.SetSchemeList(schemes);

        // Act
        _schemeRepository.Delete(scheme.Id);

        // Assert
        Assert.Empty(schemes);
    }

    [Fact]
    public void TestDeleteNonExistentScheme()
    {
        // Arrange
        var scheme = SchemeModelFactory.Create(1, "Existing Scheme", "Description", 3, [], DateTime.Now);
        List<SchemeDbModel> schemes = [SchemeDbModelFactory.Create(scheme)];
        _mockFactory.SetSchemeList(schemes);

        // Act
        _schemeRepository.Delete(2);

        // Assert
        Assert.Single(schemes);
    }

    [Fact]
    public void TestGetExistingScheme()
    {
        // Arrange
        var scheme = SchemeDbModelFactory.Create(1, "Existing Scheme", "Description", 3, DateTime.Now);

        List<SchemeDbModel> schemes = [
            scheme
        ];
        List<LabelSchemeDbModel> labelSchemes = [
            LabelSchemeDbModelFactory.Create(1, 1),
            LabelSchemeDbModelFactory.Create(2, 1),
        ];
        List<LabelDbModel> labels = [
            LabelDbModelFactory.Create(1, "Test1"),
            LabelDbModelFactory.Create(2, "Test2")
        ];
        _mockFactory.SetSchemeList(schemes);
        _mockFactory.SetLabelSchemeList(labelSchemes);
        _mockFactory.SetLabelList(labels);

        // Act
        var resultScheme = _schemeRepository.Get(scheme.Id);

        // Assert
        Assert.NotNull(resultScheme);
        Assert.Equal(scheme.Id, resultScheme.Id);
        Assert.Equal(scheme.Title, resultScheme.Title);
        Assert.Equal(scheme.Description, resultScheme.Description);
        Assert.Equal(scheme.CreatorId, resultScheme.CreatorId);
        Assert.Equal(2, resultScheme.LabelIds.Count);
        Assert.Equal(labels[0].Id, resultScheme.LabelIds[0].Id);
        Assert.Equal(labels[1].Id, resultScheme.LabelIds[1].Id);
    }

    [Fact]
    public void TestGetNonExistentScheme()
    {
        // Arrange
        List<SchemeDbModel> schemes = [];
        _mockFactory.SetSchemeList(schemes);

        // Act
        var resultScheme = _schemeRepository.Get(1);

        // Assert
        Assert.Null(resultScheme);
    }

    [Fact]
    public void TestGetAllSchemes()
    {
        // Arrange

        List<LabelSchemeDbModel> labelsSchemes = [
            LabelSchemeDbModelFactory.Create(1, 1),
            LabelSchemeDbModelFactory.Create(1, 2)
        ];
        List<LabelDbModel> labels = [
            LabelDbModelFactory.Create(1, "Test Label")
        ];
        List<SchemeDbModel> schemes = [
            SchemeDbModelFactory.Create(1, "Scheme 1", "Description 1", 3, DateTime.Now),
            SchemeDbModelFactory.Create(2, "Scheme 2", "Description 2", 4, DateTime.Now)
        ];

        _mockFactory.SetSchemeList(schemes);
        _mockFactory.SetLabelList(labels);
        _mockFactory.SetLabelSchemeList(labelsSchemes);

        // Act
        var resultSchemes = _schemeRepository.GetAll();

        // Assert
        Assert.Equal(2, resultSchemes.Count);
        Assert.Equal(schemes[0].Id, resultSchemes[0].Id);
        Assert.Equal(schemes[0].Title, resultSchemes[0].Title);
        Assert.Equal(schemes[0].Description, resultSchemes[0].Description);
        Assert.Equal(schemes[0].CreatorId, resultSchemes[0].CreatorId);
        Assert.Single(resultSchemes[0].LabelIds);
        Assert.Equal(labels[0].Id, resultSchemes[0].LabelIds[0].Id);
        Assert.Equal(schemes[1].Id, resultSchemes[1].Id);
        Assert.Equal(schemes[1].Title, resultSchemes[1].Title);
        Assert.Equal(schemes[1].Description, resultSchemes[1].Description);
        Assert.Equal(schemes[1].CreatorId, resultSchemes[1].CreatorId);
        Assert.Single(resultSchemes[1].LabelIds);
        Assert.Equal(labels[0].Id, resultSchemes[1].LabelIds[0].Id);
    }

    [Fact]
    public void TestGetAllSchemesEmpty()
    {
        // Arrange
        List<SchemeDbModel> schemes = [];
        _mockFactory.SetSchemeList(schemes);

        // Act
        var resultSchemes = _schemeRepository.GetAll();

        // Assert
        Assert.Empty(resultSchemes);
    }

    [Fact]
    public void TestUpdateExistingScheme()
    {
        // Arrange
        var scheme = SchemeDbModelFactory.Create(1, "Original Title", "Description", 3, DateTime.Now);
        List<SchemeDbModel> schemes = [scheme];
        _mockFactory.SetSchemeList(schemes);

        var updatedScheme = SchemeModelFactory.Create(1, "Updated Title", "Updated Description", 3, [], DateTime.Now);

        // Act
        _schemeRepository.Update(updatedScheme);

        // Assert
        var updatedDbScheme = schemes.FirstOrDefault(s => s.Id == updatedScheme.Id);
        Assert.NotNull(updatedDbScheme);
        Assert.Equal("Updated Title", updatedDbScheme.Title);
        Assert.Equal("Updated Description", updatedDbScheme.Description);
    }

    [Fact]
    public void TestUpdateNonExistentScheme()
    {
        // Arrange
        var scheme = SchemeDbModelFactory.Create(1, "Original Title", "Description", 3, DateTime.Now);
        List<SchemeDbModel> schemes = [scheme];
        _mockFactory.SetSchemeList(schemes);

        var updatedScheme = SchemeModelFactory.Create(2, "Updated Title", "Updated Description", 3, [], DateTime.Now);

        // Act
        _schemeRepository.Update(updatedScheme);

        // Assert
        var updatedDbScheme = schemes.FirstOrDefault(s => s.Id == updatedScheme.Id);
        Assert.Null(updatedDbScheme);
    }
}


