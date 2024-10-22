using Xunit;
using Moq;
using JustLabel.Data;
using JustLabel.Models;
using JustLabel.Exceptions;
using JustLabel.Repositories;
using JustLabel.Services;
using IntegrationTests.Data;
using IntegrationTests.Builders;

namespace IntegrationTests.Services;

[Collection("Test Database")]
public class ReportServiceIntegrationTests : BaseServiceIntegrationTests
{
    private readonly AppDbContext _context;
    private readonly ReportService _reportService;
    private readonly ReportRepository _reportRepository;
    private readonly MarkedRepository _markedRepository;
    private readonly UserRepository _userRepository;

    public ReportServiceIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _context = Fixture.CreateContext();
        _reportRepository = new ReportRepository(_context);
        _markedRepository = new MarkedRepository(_context);
        _userRepository = new UserRepository(_context);
        _reportService = new ReportService(
            _reportRepository,
            _markedRepository,
            _userRepository
        );
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
    public void TestCreateReportWithValidUserId()
    {
        using var context = Initialize();

        // Arrange
        var reportModel = new ReportModelBuilder()
            .WithId(1)
            .WithMarkedId(1)
            .WithCreatorId(1)
            .WithComment("This is a test report.")
            .Build();

        // Act
        _reportService.Create(reportModel);

        // Assert
        var reports = (from r in context.Reports select r).ToList();
        Assert.Single(reports);
        Assert.Equal(reportModel.Id, reports[0].Id);
        Assert.Equal(reportModel.MarkedId, reports[0].MarkedId);
        Assert.Equal(reportModel.CreatorId, reports[0].CreatorId);
        Assert.Equal(reportModel.Comment, reports[0].Comment);
    }

    [Fact]
    public void TestCreateReportWithInvalidUserId()
    {
        using var context = Initialize();

        // Arrange
        var reportModel = new ReportModelBuilder()
            .WithId(1)
            .WithMarkedId(1)
            .WithCreatorId(3)
            .WithComment("This is a test report.")
            .Build();

        // Act
        var exception = Assert.Throws<ReportException>(() => _reportService.Create(reportModel));

        // Assert
        Assert.Equal("CreatorId does not exist in the users list", exception.Message);
    }

    [Fact]
    public void TestGetAllReportsWhenReportsExist()
    {
        using var context = Initialize();

        // Arrange
        var report1 = new ReportDbModelBuilder().WithId(1).WithMarkedId(1).WithCreatorId(1).Build();
        var report2 = new ReportDbModelBuilder().WithId(2).WithMarkedId(1).WithCreatorId(1).Build();

        context.Reports.Add(report1);
        context.Reports.Add(report2);
        context.SaveChanges();

        // Act
        var result = _reportService.Get();

        // Assert
        var reports = (from r in context.Reports select r).ToList();
        Assert.Equal(2, result.Count);
        Assert.Equal(report1.Id, result[0].Id);
        Assert.Equal(report2.Id, result[1].Id);
    }

    [Fact]
    public void TestGetAllReportsWhenNoReportsExist()
    {
        using var context = Initialize();

        // Arrange

        // Act
        var result = _reportService.Get();

        // Assert
        Assert.Empty(result);
    }
}

