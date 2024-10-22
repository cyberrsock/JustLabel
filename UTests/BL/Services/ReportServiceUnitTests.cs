using Xunit;
using Moq;
using JustLabel.Models;
using JustLabel.Exceptions;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services;
using UnitTests.Builders;

namespace UnitTests.Services;

internal class InMemoryReportRepository: IReportRepository
{
    public List<ReportModel> Reports = [];

    public void Create(ReportModel model) {
        Reports.Add(model);
    }

    public List<ReportModel> GetAll() {
        return Reports;
    }
}

internal class InMemoryMarkedRepository : IMarkedRepository
{
    public List<MarkedModel> Marks = [];

    public void Create(MarkedModel model)
    {
        Marks.Add(model);
    }

    public void Delete(int id)
    {
        Marks.RemoveAll(x => x.Id == id);
    }

    public MarkedModel Get(MarkedModel model)
    {
        return Marks.FirstOrDefault(u => u.ImageId == model.ImageId &&
                                                        u.CreatorId == model.CreatorId &&
                                                        u.SchemeId == model.SchemeId)!;
    }

    public List<MarkedModel> GetAll()
    {
        return Marks;
    }

    public List<AggregatedModel> GetAggregatedData(int datasetId, int schemeId)
    {
        throw new NotImplementedException();
    }
    public List<MarkedModel> Get_By_DatasetId(int id)
    {
        throw new NotImplementedException();
    }
    public List<MarkedModel> Get_By_Dataset_and_SchemeId(int datasetId, int schemeId)
    {
        throw new NotImplementedException();
    }
    public List<MarkedModel> Get_By_SchemeId(int id)
    {
        throw new NotImplementedException();
    }
    public void Update_Block(MarkedModel model)
    {
        throw new NotImplementedException();
    }
    public void Update_Rects(MarkedModel model)
    {
        throw new NotImplementedException();
    }
}

internal class InMemoryUserRepository : IUserRepository
{
    public List<UserModel> Users = [];

    public void Add(UserModel model)
    {
        Users.Add(model);
    }

    public UserModel? GetUserById(int id)
    {
        return Users.FirstOrDefault(u => u.Id == id);
    }

    public void Ban(BannedModel model)
    {
        throw new NotImplementedException();
    }
    public void BanMarks(int id, bool isBlock)
    {
        throw new NotImplementedException();
    }
    public List<UserModel> GetAll()
    {
        throw new NotImplementedException();
    }
    public UserModel? GetUserByEmail(string email)
    {
        throw new NotImplementedException();
    }
    public UserModel? GetUserByUsername(string username)
    {
        throw new NotImplementedException();
    }
    public BannedModel? IsBan(int id)
    {
        throw new NotImplementedException();
    }
    public void Unban(int id)
    {
        throw new NotImplementedException();
    }
    public void UnbanByBanId(int user_id, int ban_id)
    {
        throw new NotImplementedException();
    }
    public void UpdateToken(UserModel model)
    {
        throw new NotImplementedException();
    }
}

public class ReportServiceUnitTests
{
    private readonly ReportService _reportService;
    private readonly InMemoryReportRepository _reportRepository;
    private readonly InMemoryMarkedRepository _markedRepository;
    private readonly InMemoryUserRepository _userRepository;

    public ReportServiceUnitTests()
    {
        _reportRepository = new();
        _markedRepository = new();
        _userRepository = new();
        _reportService = new ReportService(
            _reportRepository,
            _markedRepository,
            _userRepository
        );
    }

    [Fact]
    public void TestCreateReportWithValidUserId()
    {
        // Arrange
        var reportModel = new ReportModelBuilder()
            .WithId(1)
            .WithMarkedId(2)
            .WithCreatorId(3)
            .WithComment("This is a test report.")
            .Build();

        var userModel = new UserModelBuilder()
            .WithId(3)
            .Build();
        
        _userRepository.Users =[userModel];

        // Act
        _reportService.Create(reportModel);

        // Assert
        Assert.Single(_reportRepository.Reports);
        Assert.Equal(reportModel.Id, _reportRepository.Reports[0].Id);
        Assert.Equal(reportModel.MarkedId, _reportRepository.Reports[0].MarkedId);
        Assert.Equal(reportModel.CreatorId, _reportRepository.Reports[0].CreatorId);
        Assert.Equal(reportModel.Comment, _reportRepository.Reports[0].Comment);
    }

    [Fact]
    public void TestCreateReportWithInvalidUserId()
    {
        // Arrange
        var reportModel = new ReportModelBuilder()
            .WithId(1)
            .WithMarkedId(2)
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
        // Arrange
        var reportList = new List<ReportModel>
        {
            new ReportModelBuilder().WithId(1).WithMarkedId(1).WithCreatorId(2).Build(),
            new ReportModelBuilder().WithId(2).WithMarkedId(2).WithCreatorId(3).Build(),
        };

        _reportRepository.Reports = reportList;

        // Act
        var result = _reportService.Get();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(reportList[0].Id, result[0].Id);
        Assert.Equal(reportList[1].Id, result[1].Id);
    }

    [Fact]
    public void TestGetAllReportsWhenNoReportsExist()
    {
        // Arrange

        // Act
        var result = _reportService.Get();

        // Assert
        Assert.Empty(result);
    }
}

