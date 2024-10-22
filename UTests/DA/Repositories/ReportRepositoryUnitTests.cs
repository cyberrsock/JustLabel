using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using UnitTests.Data;
using UnitTests.Factories;

namespace UnitTests.Repositories;

public class ReportRepositoryUnitTests
{
    private readonly ReportRepository _reportRepository;
    private readonly MockDbContextFactory _mockFactory;

    public ReportRepositoryUnitTests()
    {
        _mockFactory = new MockDbContextFactory();
        _reportRepository = new ReportRepository(_mockFactory.MockContext.Object);
    }

    [Fact]
    public void TestCreateReportInEmptyTable()
    {
        // Arrange
        var report = ReportModelFactory.Create(
            1,
            5,
            3,
            "This is a test report.",
            DateTime.Now
        );

        List<ReportDbModel> reports = [];
        _mockFactory.SetReportList(reports);

        // Act
        _reportRepository.Create(report);

        // Assert
        Assert.Single(reports);
        Assert.Equal(report.MarkedId, reports[0].MarkedId);
        Assert.Equal(report.CreatorId, reports[0].CreatorId);
        Assert.Equal(report.Comment, reports[0].Comment);
        Assert.True((report.LoadDatetime - reports[0].LoadDatetime).TotalSeconds < 1);
    }

    [Fact]
    public void TestCreateReportInNonEmptyTable()
    {
        // Arrange
        var report1 = ReportDbModelFactory.Create(
            1,
            6,
            4,
            "Another test report.",
            DateTime.Now
        );

        var report2 = ReportModelFactory.Create(
            2,
            6,
            4,
            "Another test report.",
            DateTime.Now
        );

        List<ReportDbModel> reports = [report1];
        _mockFactory.SetReportList(reports);

        // Act
        _reportRepository.Create(report2);

        // Assert
        Assert.Equal(2, reports.Count);
        Assert.Equal(report2.MarkedId, reports[1].MarkedId);
        Assert.Equal(report2.CreatorId, reports[1].CreatorId);
        Assert.Equal(report2.Comment, reports[1].Comment);
    }

    [Fact]
    public void TestGetAllReports()
    {
        // Arrange
        var reportDbModel1 = ReportDbModelFactory.Create(1, 1, 1, "First report", DateTime.Now);
        var reportDbModel2 = ReportDbModelFactory.Create(2, 2, 2, "Second report", DateTime.Now);

        List<ReportDbModel> reports = [reportDbModel1, reportDbModel2];
        _mockFactory.SetReportList(reports);

        // Act
        var resultReports = _reportRepository.GetAll();

        // Assert
        Assert.Equal(2, resultReports.Count);
        Assert.Equal(reportDbModel1.MarkedId, resultReports[0].MarkedId);
        Assert.Equal(reportDbModel1.CreatorId, resultReports[0].CreatorId);
        Assert.Equal(reportDbModel1.Comment, resultReports[0].Comment);
        Assert.Equal(reportDbModel2.MarkedId, resultReports[1].MarkedId);
        Assert.Equal(reportDbModel2.CreatorId, resultReports[1].CreatorId);
        Assert.Equal(reportDbModel2.Comment, resultReports[1].Comment);
    }

    [Fact]
    public void TestGetNoReports()
    {
        // Arrange
        List<ReportDbModel> reports = [];
        _mockFactory.SetReportList(reports);

        // Act
        var resultReports = _reportRepository.GetAll();

        // Assert
        Assert.Empty(resultReports);
    }
}