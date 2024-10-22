using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using IntegrationTests.Data;
using IntegrationTests.Factories;
using IntegrationTests.Builders;

namespace IntegrationTests.Repositories;

[Collection("Test Database")]
public class SchemeRepositoryIntegrationTests : BaseRepositoryIntegrationTests
{
    private readonly SchemeRepository _schemeRepository;

    public SchemeRepositoryIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _schemeRepository = new (Fixture.CreateContext());
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
    public void TestAddSchemeWithoutLabels()
    {
        using var context = Initialize();

        // Arrange
        var scheme = SchemeModelFactory.Create(
            1,
            "Test Scheme",
            "This is a test scheme.",
            1,
            [],
            DateTime.Now
        );

        // Act
        _schemeRepository.Add(scheme);

        // Assert
        var schemes = (from s in context.Schemes select s).ToList();
        var labelSchemes = (from ls in context.LabelsSchemes select ls).ToList();
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
        using var context = Initialize();

        // Arrange
        var scheme = SchemeModelFactory.Create(
            1,
            "Test Scheme with Labels",
            "This is a test scheme.",
            1,
            [
                LabelModelFactory.Create(1, "Test1"),
                LabelModelFactory.Create(2, "Test2")
            ],
            DateTime.Now
        );

        context.Labels.Add(LabelDbModelFactory.Create(1, "Test1"));
        context.Labels.Add(LabelDbModelFactory.Create(2, "Test2"));
        context.SaveChanges();

        // Act
        _schemeRepository.Add(scheme);

        // Assert
        var schemes = (from s in context.Schemes select s).ToList();
        var labelSchemes = (from ls in context.LabelsSchemes select ls).ToList();
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
        using var context = Initialize();

        // Arrange
        var scheme = SchemeModelFactory.Create(1, "Scheme to Delete", "Description", 1, [], DateTime.Now);
        
        context.Schemes.Add(SchemeDbModelFactory.Create(scheme));
        context.SaveChanges();

        // Act
        _schemeRepository.Delete(scheme.Id);

        // Assert
        var schemes = (from s in context.Schemes select s).ToList();
        Assert.Empty(schemes);
    }

    [Fact]
    public void TestDeleteNonExistentScheme()
    {
        using var context = Initialize();

        // Arrange
        var scheme = SchemeModelFactory.Create(1, "Existing Scheme", "Description", 1, [], DateTime.Now);
        context.Schemes.Add(SchemeDbModelFactory.Create(scheme));
        context.SaveChanges();

        // Act
        _schemeRepository.Delete(2);

        // Assert
        var schemes = (from s in context.Schemes select s).ToList();
        Assert.Single(schemes);
    }

    [Fact]
    public void TestGetExistingScheme()
    {
        using var context = Initialize();

        // Arrange
        var scheme = SchemeDbModelFactory.Create(1, "Existing Scheme", "Description", 1, DateTime.Now);

        List<SchemeDbModel> schemes = [
            scheme
        ];
        List<LabelSchemeDbModel> labelSchemes = [
            LabelSchemeDbModelFactory.Create(1, 1),
            LabelSchemeDbModelFactory.Create(2, 1),
        ];

        context.Schemes.Add(scheme);
        context.LabelsSchemes.Add(labelSchemes[0]);
        context.LabelsSchemes.Add(labelSchemes[1]);
        context.Labels.Add(LabelDbModelFactory.Create(1, "Test1"));
        context.Labels.Add(LabelDbModelFactory.Create(2, "Test2"));
        context.SaveChanges();

        // Act
        var resultScheme = _schemeRepository.Get(scheme.Id);

        // Assert
        var labels = (from l in context.Labels select l).ToList();
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
        using var context = Initialize();

        // Arrange

        // Act
        var resultScheme = _schemeRepository.Get(1);

        // Assert
        Assert.Null(resultScheme); 
    }

    [Fact]
    public void TestGetAllSchemes()
    {
        using var context = Initialize();

        // Arrange
        List<LabelSchemeDbModel> labelSchemes = [
            LabelSchemeDbModelFactory.Create(1, 1),
            LabelSchemeDbModelFactory.Create(1, 2)
        ];

        context.Schemes.Add(SchemeDbModelFactory.Create(1, "Scheme 1", "Description 1", 1, DateTime.Now));
        context.Schemes.Add(SchemeDbModelFactory.Create(2, "Scheme 2", "Description 2", 1, DateTime.Now));
        context.LabelsSchemes.Add(labelSchemes[0]);
        context.LabelsSchemes.Add(labelSchemes[1]);
        context.Labels.Add(LabelDbModelFactory.Create(1, "Test Label"));
        context.SaveChanges();

        // Act
        var resultSchemes = _schemeRepository.GetAll();

        // Assert
        var schemes = (from s in context.Schemes select s).ToList();
        var labels = (from l in context.Labels select l).ToList();
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
        using var context = Initialize();

        // Arrange

        // Act
        var resultSchemes = _schemeRepository.GetAll();

        // Assert
        Assert.Empty(resultSchemes);
    }

    [Fact]
    public void TestUpdateNonExistentScheme()
    {
        using var context = Initialize();

        // Arrange
        var scheme = SchemeDbModelFactory.Create(1, "Original Title", "Description", 1, DateTime.Now);
        context.Schemes.Add(scheme);
        context.SaveChanges();

        var updatedScheme = SchemeModelFactory.Create(2, "Updated Title", "Updated Description", 1, [], DateTime.Now);

        // Act
        _schemeRepository.Update(updatedScheme);

        // Assert
        var updatedDbScheme = (from s in context.Schemes select s).ToList()[0];
        Assert.Equal("Original Title", updatedDbScheme.Title);
        Assert.Equal("Description", updatedDbScheme.Description);
    }
}


