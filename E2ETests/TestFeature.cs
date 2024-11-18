using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using JustLabel;
using Newtonsoft.Json;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Xunit;

namespace E2ETests;

[TestFixture]
[Binding]
public class FeatureSteps : IClassFixture<PgWebApplicationFactory<Program>>
{
    private readonly PgWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private HttpResponseMessage _response;

    public FeatureSteps(PgWebApplicationFactory<Program> factory)
    {
        _response = new HttpResponseMessage();
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_factory.jwtToken);
    }

    [Then(@"the response on (.*) should match json: (.*)")]
    public void ThenTheResponseShouldMatchJson(string endpoint, string expectedJson)
    {
        var responseBody = _response.Content.ReadAsStringAsync().Result;
        dynamic expected = JsonConvert.DeserializeObject(expectedJson); //Deserialize expected JSON

        dynamic actual = JsonConvert.DeserializeObject(responseBody); // Deserialize actual response

        actual.Should().BeEquivalentTo(expected);

    }


    [When(@"User send ""(.*)"" request to ""(.*)""")]
    public async Task WhenUserSendRequest(string method, string endpoint)
    {
        
        if (method == "PATCH")
        {
            var requestBody = new
            {
                username = "test123",
                email = "test123", 
                password = "test123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            _response = await _client.PatchAsync(endpoint, content);
        } else if (method == "PUT")
        {
            var requestBody = new
            {
                password = "test123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            _response = await _client.PutAsync(endpoint, content);
        }
    }

    [Then(@"the response on (.*) code should be (.*)")]
    public void ThenTheResponseCodeShouldBe(string endpoint, int statusCode)
    {
        _response.StatusCode.Should().Be((System.Net.HttpStatusCode)statusCode);
    }
}
