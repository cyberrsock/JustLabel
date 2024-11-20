using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using IntegrationTests.Data;
using IntegrationTests.Factories;
using IntegrationTests.Builders;
using JustLabel.Models;

namespace IntegrationTests.Repositories;

[Collection("Test Database")]
public class MarkedRepositoryIntegrationTests : BaseRepositoryIntegrationTests
{
    private readonly MarkedRepository _markedRepository;

    public MarkedRepositoryIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _markedRepository = new(Fixture.CreateContext());
    }

    private JustLabel.Data.AppDbContext Initialize()
    {
        var context = Fixture.CreateContext();

        var user1 = new UserDbModelBuilder()
            .WithId(1)
            .Build();

        context.Users.Add(user1);

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
    public void TestCreateMarkedWithoutAreas()
    {
        using var context = Initialize();

        // Arrange
        var markedModel = MarkedModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now,
            [
                // AreaModelFactory.Create(1, 1, [(1,2), (2,3)])
            ]
        );

        // Act
        _markedRepository.Create(markedModel);

        // Assert
        var marks = (from m in context.Marked select m).ToList();
        Assert.Single(marks);
        Assert.Equal(markedModel.ImageId, marks[0].ImageId);
        Assert.Equal(markedModel.SchemeId, marks[0].SchemeId);
    }

    [Fact]
    public void TestCreateMarkedWithAreas()
    {
        using var context = Initialize();

        // Arrange
        var markedModel = MarkedModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now,
            [
                AreaModelFactory.Create(1, 1, [(1, 2), (2, 3)]),
                AreaModelFactory.Create(2, 1, [(1, 2), (2, 3)]),
            ]
        );

        // Act
        _markedRepository.Create(markedModel);

        // Assert
        var marks = (from m in context.Marked select m).ToList();
        var areas = (from a in context.Areas select a).ToList();
        Assert.Single(marks);
        Assert.Equal(2, areas.Count);
    }

    [Fact]
    public void TestDeleteExistingMarked()
    {
        using var context = Initialize();

        // Arrange
        var markedDbModel = MarkedDbModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now
        );
        context.Marked.Add(markedDbModel);
        context.SaveChanges();

        // Act
        _markedRepository.Delete(1);

        // Assert
        var marks = (from m in context.Marked select m).ToList();
        Assert.Empty(marks);
    }

    [Fact]
    public void TestDeleteNonExistentMarked()
    {
        using var context = Initialize();

        // Arrange
        var markedDbModel = MarkedDbModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now
        );
        context.Marked.Add(markedDbModel);
        context.SaveChanges();

        // Act
        _markedRepository.Delete(2);

        // Assert
        var marks = (from m in context.Marked select m).ToList();
        Assert.Single(marks);
    }

    [Fact]
    public void TestGetByDatasetId()
    {
        using var context = Initialize();

        // Arrange
        var markedDbModel = MarkedDbModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now
        );

        context.Marked.Add(markedDbModel);
        context.SaveChanges();

        // Act
        var result = _markedRepository.Get_By_DatasetId(1);

        // Assert
        Assert.Single(result);
        Assert.Equal(markedDbModel.ImageId, result[0].ImageId);
    }

    [Fact]
    public void TestGetByNonExistentDatasetId()
    {
        using var context = Initialize();

        // Arrange
        var markedDbModel = MarkedDbModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now
        );

        context.Marked.Add(markedDbModel);
        context.SaveChanges();

        // Act
        var result = _markedRepository.Get_By_DatasetId(2);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void TestGetBySchemeId()
    {
        using var context = Initialize();

        // Arrange
        var markedDbModel = MarkedDbModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now
        );

        context.Marked.Add(markedDbModel);
        context.SaveChanges();

        // Act
        var result = _markedRepository.Get_By_SchemeId(1);

        // Assert
        Assert.Single(result);
        Assert.Equal(markedDbModel.SchemeId, result[0].SchemeId);
    }

    [Fact]
    public void TestGetByNonExistentSchemeId()
    {
        using var context = Initialize();

        // Arrange
        var markedDbModel = MarkedDbModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now
        );

        context.Marked.Add(markedDbModel);
        context.SaveChanges();

        // Act
        var result = _markedRepository.Get_By_SchemeId(100);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void TestGetByDatasetAndSchemeId()
    {
        using var context = Initialize();

        // Arrange
        var markedDbModel = MarkedDbModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now
        );

        context.Marked.Add(markedDbModel);
        context.SaveChanges();

        // Act
        var result = _markedRepository.Get_By_Dataset_and_SchemeId(1, 1);

        // Assert
        Assert.Single(result);
        Assert.Equal(markedDbModel.Id, result[0].Id);
    }

    [Fact]
    public void TestGetByNonExistentDatasetAndSchemeId()
    {
        using var context = Initialize();

        // Arrange
        var markedDbModel = MarkedDbModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now
        );

        context.Marked.Add(markedDbModel);
        context.SaveChanges();

        // Act
        var result = _markedRepository.Get_By_Dataset_and_SchemeId(3, 5);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void TestGetNonExistentMarked()
    {
        using var context = Initialize();

        // Arrange
        var markedModel = MarkedModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now,
            [
                AreaModelFactory.Create(1, 1, [(1, 2), (2, 3)]),
                AreaModelFactory.Create(2, 1, [(1, 2), (2, 3)]),
            ]
        );

        // Act
        var result = _markedRepository.Get(markedModel);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void TestGetExistingMarked()
    {
        using var context = Initialize();

        // Arrange
        var markedDbModel = MarkedDbModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now
        );

        context.Marked.Add(markedDbModel);
        context.SaveChanges();

        var markedModel = MarkedModelFactory.Create(markedDbModel);

        // Act
        var result = _markedRepository.Get(markedModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(markedDbModel.Id, result.Id);
        Assert.Equal(markedDbModel.ImageId, result.ImageId);
    }

    [Fact]
    public void TestUpdateRects()
    {
        using var context = Initialize();

        // Arrange
        var markedModel = MarkedModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now,
            [
                AreaModelFactory.Create(1, 1, [(1, 2), (2, 3)])
            ]
        );

        context.Marked.Add(MarkedDbModelFactory.Create(markedModel));
        context.SaveChanges();

        // Act
        _markedRepository.Update_Rects(markedModel);

        // Assert
        var areas = (from a in context.Areas select a).ToList();
        Assert.Single(areas);
        Assert.Equal(areas[0].Id, markedModel.AreaModels[0].Id);
        Assert.Equal(areas[0].LabelId, markedModel.AreaModels[0].LabelId);
    }

    [Fact]
    public void TestUpdateNonExistentRects()
    {
        using var context = Initialize();

        // Arrange
        var markedModel = MarkedModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now,
            [
                AreaModelFactory.Create(1, 1, [(1, 2), (2, 3)])
            ]
        );
        List<MarkedDbModel> marks = [
            MarkedDbModelFactory.Create(
                1,
                1,
                1,
                1,
                false,
                DateTime.Now
            )
        ];

        context.Marked.Add(marks[0]);
        context.SaveChanges();

        // Act
        _markedRepository.Update_Rects(markedModel);

        // Assert
        Assert.Single(marks);
    }

    [Fact]
    public void TestUpdateBlockWithNonExistentId()
    {
        using var context = Initialize();

        // Arrange
        List<MarkedDbModel> marks = [
            MarkedDbModelFactory.Create(
                1,
                1,
                1,
                1,
                false,
                DateTime.Now
            )
        ];

        context.Marked.Add(marks[0]);
        context.SaveChanges();

        var markedModel = MarkedModelFactory.Create(marks[0]);
        markedModel.IsBlocked = true;
        markedModel.Id = 2;

        // Act
        _markedRepository.Update_Block(markedModel);

        // Assert
        Assert.False(marks[0].IsBlocked);
    }

    [Fact]
    public void TestGetAll()
    {
        using var context = Initialize();

        // Arrange
        var markedDbModel1 = MarkedDbModelFactory.Create(
            1,
            1,
            1,
            1,
            false,
            DateTime.Now
        );
        var markedDbModel2 = MarkedDbModelFactory.Create(
            2,
            1,
            1,
            1,
            false,
            DateTime.Now
        );

        context.Marked.Add(markedDbModel1);
        context.Marked.Add(markedDbModel2);
        context.SaveChanges();

        // Act
        var result = _markedRepository.GetAll();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, m => m.Id == markedDbModel1.Id);
        Assert.Contains(result, m => m.Id == markedDbModel2.Id);
    }

    [Fact]
    public void TestGetAllNoMarked()
    {
        using var context = Initialize();

        // Arrange

        // Act
        var result = _markedRepository.GetAll();

        // Assert
        Assert.Empty(result);
    }
}


