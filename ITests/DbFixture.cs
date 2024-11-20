using JustLabel.Data;
using JustLabel.Data.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests.Data;

public class DatabaseFixture
{
    private string ConnectionString =
        $"Host=localhost;Port=5432;Username=postgres;Password=123;Database=testdb";

    public AppDbContext CreateContext() =>
        new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(ConnectionString)
                .EnableSensitiveDataLogging()
                .Options
        );

    public DatabaseFixture()
    {
        using var context = CreateContext();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Cleanup();
    }

    public void Cleanup()
    {
        using var context = CreateContext();

        context.Datasets.RemoveRange(context.Datasets);
        context.Images.RemoveRange(context.Images);
        context.Labels.RemoveRange(context.Labels);
        context.Marked.RemoveRange(context.Marked);
        context.Reports.RemoveRange(context.Reports);
        context.Schemes.RemoveRange(context.Schemes);
        context.LabelsSchemes.RemoveRange(context.LabelsSchemes);
        context.MarkedAreas.RemoveRange(context.MarkedAreas);
        context.Areas.RemoveRange(context.Areas);
        context.Users.RemoveRange(context.Users);
        context.Banned.RemoveRange(context.Banned);
        context.SaveChanges();
    }
}

[CollectionDefinition("Test Database", DisableParallelization = false)]
[assembly: CollectionBehavior(MaxParallelThreads = 8)]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }
