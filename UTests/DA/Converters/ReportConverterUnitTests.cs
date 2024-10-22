using Xunit;
using JustLabel.Data.Converters;
using UnitTests.Builders;

namespace UnitTests.Converters;

public class ReportConverterUnitTests
{
    [Fact]
    public void TestConvertOkCoreToDbModel()
    {
        // Arrange
        var report = new ReportModelBuilder()
            .WithId(1)
            .WithMarkedId(2)
            .WithCreatorId(3)
            .WithComment("This is a sample comment.")
            .WithLoadDatetime(DateTime.Now)
            .Build();

        // Act
        var reportDb = ReportConverter.CoreToDbModel(report);

        // Assert
        Assert.Equal(report.Id, reportDb.Id);
        Assert.Equal(report.MarkedId, reportDb.MarkedId);
        Assert.Equal(report.CreatorId, reportDb.CreatorId);
        Assert.Equal(report.Comment, reportDb.Comment);
        Assert.Equal(report.LoadDatetime, reportDb.LoadDatetime);
    }

    [Fact]
    public void TestConvertOkDbToCoreModel()
    {
        // Arrange
        var reportDb = new ReportDbModelBuilder()
            .WithId(1)
            .WithMarkedId(2)
            .WithCreatorId(3)
            .WithComment("This is a sample comment.")
            .WithLoadDatetime(DateTime.Now)
            .Build();

        // Act
        var report = ReportConverter.DbToCoreModel(reportDb);

        // Assert
        Assert.Equal(reportDb.Id, report.Id);
        Assert.Equal(reportDb.MarkedId, report.MarkedId);
        Assert.Equal(reportDb.CreatorId, report.CreatorId);
        Assert.Equal(reportDb.Comment, report.Comment);
        Assert.Equal(reportDb.LoadDatetime, report.LoadDatetime);
    }

    [Fact]
    public void TestConvertNullCoreToDbModel()
    {
        // Arrange

        // Act
        var reportDb = ReportConverter.CoreToDbModel(null);

        // Assert
        Assert.Null(reportDb);
    }

    [Fact]
    public void TestConvertNullDbToCoreModel()
    {
        // Arrange

        // Act
        var report = ReportConverter.DbToCoreModel(null);

        // Assert
        Assert.Null(report);
    }
}
