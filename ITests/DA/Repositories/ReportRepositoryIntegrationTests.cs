using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using IntegrationTests.Data;
using IntegrationTests.Factories;
using IntegrationTests.Builders;

namespace IntegrationTests.Repositories;

[Collection("Test Database")]
public class ReportRepositoryIntegrationTests : BaseRepositoryIntegrationTests
{
    private readonly ReportRepository _reportRepository;

    public ReportRepositoryIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _reportRepository = new(Fixture.CreateContext());
    }


    private JustLabel.Data.AppDbContext Initialize()
    {
        var context = Fixture.CreateContext();

        var user1 = new UserDbModelBuilder()
            .WithId(1)
            .Build();

        var user2 = new UserDbModelBuilder()
            .WithId(2)
            .Build();

        context.Users.Add(user1);
        context.Users.Add(user2);

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

        var mark1 = new MarkedDbModelBuilder()
            .WithId(1)
            .WithImageId(1)
            .WithSchemeId(1)
            .WithCreatorId(1)
            .Build();

        context.Marked.Add(mark1);

        context.SaveChanges();

        return context;
    }

    [Fact]
    public void TestCreateReportInEmptyTable()
    {
        using var context = Initialize();

        // Arrange
        var report = ReportModelFactory.Create(
            1,
            1,
            2,
            "This is a test report.",
            DateTime.Now
        );

        // Act
        _reportRepository.Create(report);

        // Assert
        var reports = (from r in context.Reports select r).ToList();
        Assert.Single(reports);
        Assert.Equal(report.MarkedId, reports[0].MarkedId);
        Assert.Equal(report.CreatorId, reports[0].CreatorId);
        Assert.Equal(report.Comment, reports[0].Comment);
        Assert.True((report.LoadDatetime - reports[0].LoadDatetime).TotalSeconds < 1);
    }

    [Fact]
    public void TestCreateReportInNonEmptyTable()
    {
        using var context = Initialize();

        // Arrange
        var report1 = ReportDbModelFactory.Create(
            1,
            1,
            2,
            "Another test report.",
            DateTime.Now
        );

        var report2 = ReportModelFactory.Create(
            2,
            1,
            2,
            "Another test report.",
            DateTime.Now
        );

        context.Reports.Add(report1);
        context.SaveChanges();

        // Act
        _reportRepository.Create(report2);

        // Assert
        var reports = (from r in context.Reports select r).ToList();
        Assert.Equal(2, reports.Count);
        Assert.Equal(report2.MarkedId, reports[1].MarkedId);
        Assert.Equal(report2.CreatorId, reports[1].CreatorId);
        Assert.Equal(report2.Comment, reports[1].Comment);
    }

    [Fact]
    public void TestGetAllReports()
    {
        using var context = Initialize();

        // Arrange
        var reportDbModel1 = ReportDbModelFactory.Create(1, 1, 2, "First report", DateTime.Now);
        var reportDbModel2 = ReportDbModelFactory.Create(2, 1, 2, "Second report", DateTime.Now);

        context.Reports.Add(reportDbModel1);
        context.Reports.Add(reportDbModel2);
        context.SaveChanges();

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
        using var context = Initialize();

        // Arrange

        // Act
        var resultReports = _reportRepository.GetAll();

        // Assert
        Assert.Empty(resultReports);
    }
}