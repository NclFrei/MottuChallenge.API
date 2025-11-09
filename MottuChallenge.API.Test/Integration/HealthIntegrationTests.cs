using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MottuChallenge.API.Test.Integration;

public class HealthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
 private readonly WebApplicationFactory<Program> _factory;

 public HealthIntegrationTests(WebApplicationFactory<Program> factory)
 {
 _factory = factory;
 }

 [Fact]
 public async Task HealthEndpoint_ReturnsOk()
 {
 var client = _factory.CreateClient();
 var response = await client.GetAsync("/health");

 Assert.True(response.IsSuccessStatusCode);
 }

 [Fact]
 public async Task HealthDetailsEndpoint_ReturnsDatabaseCheck()
 {
 var client = _factory.CreateClient();
 var response = await client.GetAsync("/health/details");

 Assert.True(response.IsSuccessStatusCode);

 var content = await response.Content.ReadAsStringAsync();
 Assert.Contains("database", content.ToLower());
 }
}
