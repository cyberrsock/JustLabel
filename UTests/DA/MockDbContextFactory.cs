using JustLabel.Data;
using JustLabel.Data.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Data;

public class MockDbContextFactory
{
    public Mock<AppDbContext> MockContext { get; set; }

    public Mock<DbSet<DatasetDbModel>> MockDatasetsDbSet { get; set; }
    public Mock<DbSet<ImageDbModel>> MockImagesDbSet { get; set; }
    public Mock<DbSet<LabelDbModel>> MockLabelsDbSet { get; set; }
    public Mock<DbSet<MarkedDbModel>> MockMarkedDbSet { get; set; }
    public Mock<DbSet<ReportDbModel>> MockReportsDbSet { get; set; }
    public Mock<DbSet<SchemeDbModel>> MockSchemesDbSet { get; set; }
    public Mock<DbSet<LabelSchemeDbModel>> MockLabelsSchemesDbSet { get; set; }
    public Mock<DbSet<MarkedAreaDbModel>> MockMarkedAreasDbSet { get; set; }
    public Mock<DbSet<AreaDbModel>> MockAreasDbSet { get; set; }
    public Mock<DbSet<UserDbModel>> MockUsersDbSet { get; set; }
    public Mock<DbSet<BannedDbModel>> MockBannedDbSet { get; set; }
    public Mock<DbSet<AggregatedDbModel>> MockAggregatedDbSet { get; set; }

    public MockDbContextFactory()
    {
        MockContext = new Mock<AppDbContext>();

        MockDatasetsDbSet = SetupMockDbSet(new List<DatasetDbModel>());
        MockImagesDbSet = SetupMockDbSet(new List<ImageDbModel>());
        MockLabelsDbSet = SetupMockDbSet(new List<LabelDbModel>());
        MockMarkedDbSet = SetupMockDbSet(new List<MarkedDbModel>());
        MockReportsDbSet = SetupMockDbSet(new List<ReportDbModel>());
        MockSchemesDbSet = SetupMockDbSet(new List<SchemeDbModel>());
        MockLabelsSchemesDbSet = SetupMockDbSet(new List<LabelSchemeDbModel>());
        MockMarkedAreasDbSet = SetupMockDbSet(new List<MarkedAreaDbModel>());
        MockAreasDbSet = SetupMockDbSet(new List<AreaDbModel>());
        MockUsersDbSet = SetupMockDbSet(new List<UserDbModel>());
        MockBannedDbSet = SetupMockDbSet(new List<BannedDbModel>());
        MockAggregatedDbSet = SetupMockDbSet(new List<AggregatedDbModel>());

        MockContext.Setup(m => m.Datasets).Returns(MockDatasetsDbSet.Object);
        MockContext.Setup(m => m.Images).Returns(MockImagesDbSet.Object);
        MockContext.Setup(m => m.Labels).Returns(MockLabelsDbSet.Object);
        MockContext.Setup(m => m.Marked).Returns(MockMarkedDbSet.Object);
        MockContext.Setup(m => m.Reports).Returns(MockReportsDbSet.Object);
        MockContext.Setup(m => m.Schemes).Returns(MockSchemesDbSet.Object);
        MockContext.Setup(m => m.LabelsSchemes).Returns(MockLabelsSchemesDbSet.Object);
        MockContext.Setup(m => m.MarkedAreas).Returns(MockMarkedAreasDbSet.Object);
        MockContext.Setup(m => m.Areas).Returns(MockAreasDbSet.Object);
        MockContext.Setup(m => m.Users).Returns(MockUsersDbSet.Object);
        MockContext.Setup(m => m.Banned).Returns(MockBannedDbSet.Object);
        MockContext.Setup(m => m.Aggregated).Returns(MockAggregatedDbSet.Object);

        MockContext.Setup(m => m.SaveChanges()).Callback(() => {});
    }

    public static Mock<DbSet<T>> SetupMockDbSet<T>(List<T> list)
        where T : class
    {
        var queryable = list.AsQueryable();
        var mockDbSet = new Mock<DbSet<T>>();
        mockDbSet
            .As<IQueryable<T>>()
            .Setup(m => m.Provider)
            .Returns(queryable.Provider);
        mockDbSet
            .As<IQueryable<T>>()
            .Setup(m => m.Expression)
            .Returns(queryable.Expression);
        mockDbSet
            .As<IQueryable<T>>()
            .Setup(m => m.ElementType)
            .Returns(queryable.ElementType);
        mockDbSet
            .As<IQueryable<T>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => queryable.GetEnumerator());
        mockDbSet
            .Setup(m => m.Add(It.IsAny<T>()))
            .Callback<T>(item => list.Add(item));
         mockDbSet
            .Setup(m => m.Remove(It.IsAny<T>()))
            .Callback<T>(item => list.Remove(item));
        return mockDbSet;
    }

    public void SetUserList(List<UserDbModel> users)
    {
        MockUsersDbSet = SetupMockDbSet(users);
        MockContext.Setup(m => m.Users).Returns(MockUsersDbSet.Object);
    }

    public void SetDatasetList(List<DatasetDbModel> datasets)
    {
        MockDatasetsDbSet = SetupMockDbSet(datasets);
        MockContext.Setup(m => m.Datasets).Returns(MockDatasetsDbSet.Object);
    }

    public void SetAreaList(List<AreaDbModel> areas)
    {
        MockAreasDbSet = SetupMockDbSet(areas);
        MockContext.Setup(m => m.Areas).Returns(MockAreasDbSet.Object);
    }

    public void SetImageList(List<ImageDbModel> images)
    {
        MockImagesDbSet = SetupMockDbSet(images);
        MockContext.Setup(m => m.Images).Returns(MockImagesDbSet.Object);
    }

    public void SetLabelList(List<LabelDbModel> labels)
    {
        MockLabelsDbSet = SetupMockDbSet(labels);
        MockContext.Setup(m => m.Labels).Returns(MockLabelsDbSet.Object);
    }

    public void SetMarkedList(List<MarkedDbModel> marked)
    {
        MockMarkedDbSet = SetupMockDbSet(marked);
        MockContext.Setup(m => m.Marked).Returns(MockMarkedDbSet.Object);
    }

    public void SetReportList(List<ReportDbModel> reports)
    {
        MockReportsDbSet = SetupMockDbSet(reports);
        MockContext.Setup(m => m.Reports).Returns(MockReportsDbSet.Object);
    }

    public void SetSchemeList(List<SchemeDbModel> schemes)
    {
        MockSchemesDbSet = SetupMockDbSet(schemes);
        MockContext.Setup(m => m.Schemes).Returns(MockSchemesDbSet.Object);
    }

    public void SetBannedList(List<BannedDbModel> banned)
    {
        MockBannedDbSet = SetupMockDbSet(banned);
        MockContext.Setup(m => m.Banned).Returns(MockBannedDbSet.Object);
    }

    public void SetLabelSchemeList(List<LabelSchemeDbModel> labelscheme)
    {
        MockLabelsSchemesDbSet = SetupMockDbSet(labelscheme);
        MockContext.Setup(m => m.LabelsSchemes).Returns(MockLabelsSchemesDbSet.Object);
    }

    public void SetList(List<MarkedAreaDbModel> markedarea)
    {
        MockMarkedAreasDbSet = SetupMockDbSet(markedarea);
        MockContext.Setup(m => m.MarkedAreas).Returns(MockMarkedAreasDbSet.Object);
    }
}
  