using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using JustLabel.Data.Converters;
using JustLabel.Data.Models;
using JustLabel.Models;
using UnitTests.Builders;

namespace UnitTests.Converters;

public class MarkedConverterUnitTests
{
    [Fact]
    public void TestConvertOkCoreToDbModel()
    {
        // Arrange
        var marked = new MarkedModelBuilder()
            .WithId(1)
            .WithSchemeId(10)
            .WithImageId(100)
            .WithCreatorId(42)
            .WithIsBlocked(false)
            .WithCreateDatetime(DateTime.Now)
            .Build();

        // Act
        var markedDb = MarkedConverter.CoreToDbModel(marked);

        // Assert
        Assert.Equal(marked.Id, markedDb.Id);
        Assert.Equal(marked.SchemeId, markedDb.SchemeId);
        Assert.Equal(marked.ImageId, markedDb.ImageId);
        Assert.Equal(marked.CreatorId, markedDb.CreatorId);
        Assert.Equal(marked.IsBlocked, markedDb.IsBlocked);
        Assert.Equal(marked.CreateDatetime, markedDb.CreateDatetime);
    }

    [Fact]
    public void TestConvertNullCoreToDbModel()
    {
        // Act
        var markedDb = MarkedConverter.CoreToDbModel(null);

        // Assert
        Assert.Null(markedDb);
    }

    [Fact]
    public void TestConvertOkCoreToDbConnectModel()
    {
        // Arrange
        var marked = new MarkedModelBuilder()
            .WithId(1)
            .WithAreaModels(new List<AreaModel>
            {
                new AreaModelBuilder().WithId(1).Build(),
                new AreaModelBuilder().WithId(2).Build()
            })
            .Build();

        // Act
        var markedAreaModels = MarkedConverter.CoreToDbConnectModel(marked);

        // Assert
        Assert.Equal(2, markedAreaModels.Count);
        Assert.All(markedAreaModels, markedAreaModel =>
        {
            Assert.Equal(marked.Id, markedAreaModel.MarkedId);
            Assert.Contains(marked.AreaModels, area => area.Id == markedAreaModel.AreaId);
        });
    }

    [Fact]
    public void TestConvertNullCoreToDbConnectModel()
    {
        // Act
        var markedAreaModels = MarkedConverter.CoreToDbConnectModel(null);

        // Assert
        Assert.Null(markedAreaModels);
    }

    [Fact]
    public void TestConvertOkDbToCoreModel()
    {
        // Arrange
        var markedDb = new MarkedDbModelBuilder()
            .WithId(1)
            .WithSchemeId(10)
            .WithImageId(100)
            .WithCreatorId(42)
            .WithIsBlocked(false)
            .WithCreateDatetime(DateTime.Now)
            .Build();

        var markedAreaModels = new List<MarkedAreaDbModel>
        {
            new MarkedAreaDbModelBuilder().WithAreaId(1).WithMarkedId(1).Build(),
            new MarkedAreaDbModelBuilder().WithAreaId(2).WithMarkedId(1).Build()
        };

        var areaDbModels = new List<AreaDbModel>
        {
            new AreaDbModelBuilder().WithId(1).WithLabelId(1).WithCoords(new (double X, double Y)[] { (10, 20) }).Build(),
            new AreaDbModelBuilder().WithId(2).WithLabelId(1).WithCoords(new (double X, double Y)[] { (10, 20) }).Build()
        };

        // Act
        var marked = MarkedConverter.DbToCoreModel(markedDb, markedAreaModels, areaDbModels);

        // Assert
        Assert.Equal(markedDb.Id, marked.Id);
        Assert.Equal(markedDb.SchemeId, marked.SchemeId);
        Assert.Equal(markedDb.ImageId, marked.ImageId);
        Assert.Equal(markedDb.CreatorId, marked.CreatorId);
        Assert.Equal(markedDb.IsBlocked, marked.IsBlocked);
        Assert.Equal(markedDb.CreateDatetime, marked.CreateDatetime);
        Assert.Equal(2, marked.AreaModels.Count);
    }

    [Fact]
    public void TestConvertNullDbToCoreModel()
    {
        // Act
        var marked = MarkedConverter.DbToCoreModel(null, null, null);

        // Assert
        Assert.Null(marked);
    }
}
