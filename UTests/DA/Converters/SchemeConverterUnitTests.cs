using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using JustLabel.Data.Converters;
using JustLabel.Data.Models;
using JustLabel.Models;
using UnitTests.Builders;

namespace UnitTests.Converters;

public class SchemeConverterUnitTests
{
    [Fact]
    public void TestConvertOkCoreToDbModel()
    {
        // Arrange
        var scheme = new SchemeModelBuilder()
            .WithId(1)
            .WithTitle("Sample Scheme")
            .WithDescription("This is a sample scheme.")
            .WithCreatorId(42)
            .WithCreateDatetime(DateTime.UtcNow)
            .Build();

        // Act
        var schemeDb = SchemeConverter.CoreToDbModel(scheme);

        // Assert
        Assert.Equal(scheme.Id, schemeDb.Id);
        Assert.Equal(scheme.Title, schemeDb.Title);
        Assert.Equal(scheme.Description, schemeDb.Description);
        Assert.Equal(scheme.CreatorId, schemeDb.CreatorId);
        Assert.Equal(scheme.CreateDatetime, schemeDb.CreateDatetime);
    }

    [Fact]
    public void TestConvertNullCoreToDbModel()
    {
        // Arrange

        // Act
        var schemeDb = SchemeConverter.CoreToDbModel(null);

        // Assert
        Assert.Null(schemeDb);
    }

    [Fact]
    public void TestConvertOkCoreToDbConnectModel()
    {
        // Arrange
        var scheme = new SchemeModelBuilder()
            .WithId(1)
            .WithLabelIds(new List<LabelModel>
            {
                new LabelModelBuilder().WithId(1).Build(),
                new LabelModelBuilder().WithId(2).Build()
            })
            .Build();

        // Act
        var labelSchemeDbModels = SchemeConverter.CoreToDbConnectModel(scheme);

        // Assert
        Assert.Equal(2, labelSchemeDbModels.Count);
        Assert.All(labelSchemeDbModels, labelSchemeDbModel =>
        {
            Assert.Equal(scheme.Id, labelSchemeDbModel.SchemeId);
            Assert.Contains(scheme.LabelIds, label => label.Id == labelSchemeDbModel.LabelId);
        });
    }

    [Fact]
    public void TestConvertNullCoreToDbConnectModel()
    {
        // Arrange

        // Act
        var labelSchemeDbModels = SchemeConverter.CoreToDbConnectModel(null);

        // Assert
        Assert.Null(labelSchemeDbModels);
    }

    [Fact]
    public void TestConvertOkDbToCoreModel()
    {
        // Arrange
        var schemeDb = new SchemeDbModelBuilder()
            .WithId(1)
            .WithTitle("Sample Scheme")
            .WithDescription("This is a sample scheme.")
            .WithCreatorId(42)
            .WithCreateDatetime(DateTime.UtcNow)
            .Build();

        var labelSchemeDbModels = new List<LabelSchemeDbModel>
        {
            new LabelSchemeDbModel { LabelId = 1, SchemeId = 1 },
            new LabelSchemeDbModel { LabelId = 2, SchemeId = 1 }
        };

        // Act
        var scheme = SchemeConverter.DbToCoreModel(schemeDb, labelSchemeDbModels);

        // Assert
        Assert.Equal(schemeDb.Id, scheme.Id);
        Assert.Equal(schemeDb.Title, scheme.Title);
        Assert.Equal(schemeDb.Description, scheme.Description);
        Assert.Equal(schemeDb.CreatorId, scheme.CreatorId);
        Assert.Equal(schemeDb.CreateDatetime, scheme.CreateDatetime);
        Assert.Equal(2, scheme.LabelIds.Count);
    }

    [Fact]
    public void TestConvertNullDbToCoreModel()
    {
        // Arrange

        // Act
        var scheme = SchemeConverter.DbToCoreModel(null, null);

        // Assert
        Assert.Null(scheme);
    }
}
