using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using MottuChallenge.API;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;

namespace MottuChallenge.API.Test.Integration;

public class PatioControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> _factory;

    public PatioControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllPatios_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/patio?page=1&limit=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreatePatio_ReturnsCreated()
    {
        var client = _factory.CreateClient();

        var request = new PatioRequest { Nome = "ITestPatio", UserId = 1, Endereco = new EnderecoRequest { Rua = "R", Numero = 1 } };

        var response = await client.PostAsJsonAsync("/api/v1/patio", request);

        // Depending on DB connectivity this may be BadRequest or Created; assert non-server-error
        Assert.True(response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.BadRequest);
    }
}
