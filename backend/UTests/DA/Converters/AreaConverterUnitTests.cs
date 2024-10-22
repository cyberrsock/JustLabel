using Xunit;
using JustLabel.Data.Converters;
using UnitTests.Builders;
using System.Linq;

namespace UnitTests.Converters;

public class AreaConverterUnitTests
{
    [Fact]
    public void TestConvertOkCoreToDbModel()
    {
        // Arrange
        var area = new AreaModelBuilder()
            .WithId(1)
            .WithLabelId(2)
            .WithCoords(new (double X, double Y)[] { (10, 20) })
            .Build();

        // Act
        var areaDb = AreaConverter.CoreToDbModel(area);

        // Assert
        Assert.Equal(area.Id, areaDb.Id);
        Assert.Equal(area.LabelId, areaDb.LabelId);
        Assert.Equal(area.Coords[0].X, areaDb.Coords[0].X);
        Assert.Equal(area.Coords[0].Y, areaDb.Coords[0].Y);
    }

    [Fact]
    public void TestConvertOkDbToCoreModel()
    {
        // Arrange
        var areaDb = new AreaDbModelBuilder()
            .WithId(1)
            .WithLabelId(2)
            .WithCoords(new (double X, double Y)[] { (10, 20) })
            .Build();

        // Act
        var area = AreaConverter.DbToCoreModel(areaDb);

        // Assert
        Assert.Equal(areaDb.Id, area.Id);
        Assert.Equal(areaDb.LabelId, area.LabelId);
        Assert.Equal(areaDb.Coords[0].X, area.Coords[0].X);
        Assert.Equal(areaDb.Coords[0].Y, area.Coords[0].Y);
    }

    [Fact]
    public void TestConvertNullCoreToDbModel()
    {
        // Arrange

        // Act
        var areaDb = AreaConverter.CoreToDbModel(null);

        // Assert
        Assert.Null(areaDb);
    }

    [Fact]
    public void TestConvertNullDbToCoreModel()
    {
        // Arrange

        // Act
        var area = AreaConverter.DbToCoreModel(null);

        // Assert
        Assert.Null(area);
    }
}
