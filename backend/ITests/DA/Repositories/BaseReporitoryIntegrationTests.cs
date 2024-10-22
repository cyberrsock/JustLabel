using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using IntegrationTests.Data;
using IntegrationTests.Factories;
using IntegrationTests.Builders;

namespace IntegrationTests.Repositories;

public class BaseRepositoryIntegrationTests(DatabaseFixture fixture) : IDisposable
{
    public DatabaseFixture Fixture { get; } = fixture;

    public void Dispose()
    {
        Fixture.Cleanup();
    }
}