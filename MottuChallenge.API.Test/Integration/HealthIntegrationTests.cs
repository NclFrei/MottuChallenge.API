using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using MottuChallenge.API;

namespace MottuChallenge.API.Test.Integration;

public class HealthIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task HealthEndpoint_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
