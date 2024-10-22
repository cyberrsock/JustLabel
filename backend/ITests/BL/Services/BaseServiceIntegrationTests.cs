using Xunit;
using Moq;
using JustLabel.Models;
using JustLabel.Exceptions;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services;
using JustLabel.Utilities;
using IntegrationTests.Data;
using IntegrationTests.Builders;

namespace IntegrationTests.Services;

[Collection("Test Database")]
public class BaseServiceIntegrationTests(DatabaseFixture fixture) : IDisposable
{
    public DatabaseFixture Fixture { get; } = fixture;

    public void Dispose()
    {
        Fixture.Cleanup();
    }
}
