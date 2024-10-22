using Xunit;
using System.Net;
using System.Text;
using System.Text.Json;
using JustLabel;
using JustLabel.Data;
using JustLabel.Models;
using JustLabel.Exceptions;
using JustLabel.Repositories;
using JustLabel.Services;
using Newtonsoft.Json;
using JustLabel.DTOModels;

namespace E2ETests;

public class TestMvp : IClassFixture<PgWebApplicationFactory<Program>>
{
    private readonly PgWebApplicationFactory<Program> _factory;

    public TestMvp(PgWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task RegisterUser_Ok()
    {
        var client = _factory.CreateClient();

        var request = new AuthDTOModel()
        {
            Username = "123456789",
            Email = "123456789",
            Password = "123456789",
        };
        var httpContent = new StringContent(
            JsonConvert.SerializeObject(request),
            Encoding.UTF8,
            "application/json"
        );
        var response = await client.PostAsync(
            "/api/v2/auth",
            httpContent
        );

        var str = await response.Content.ReadAsStringAsync();

        string? token;
        using (JsonDocument doc = JsonDocument.Parse(str))
        {
            JsonElement root = doc.RootElement;
            token = root.GetProperty("accessToken").GetString()!;
        }
        client.DefaultRequestHeaders.Add("Authorization", token);

        var datasets = await client.GetAsync($"/api/v2/datasets");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using (
            JsonDocument doc = JsonDocument.Parse(
                await datasets.Content.ReadAsStringAsync()
            )
        )
        {
            JsonElement root = doc.RootElement;

            Assert.Equal(0, root.GetArrayLength());
        }
    }
}
