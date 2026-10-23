using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RaceHub.Api.Tests;

public sealed class RaceEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public RaceEndpointsTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Fact]
    public async Task ListRaces_ReturnsSeededEvents()
    {
        var response = await _client.GetAsync("/api/races");
        var races = await response.Content.ReadFromJsonAsync<List<RaceSummaryContract>>();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(races);
        Assert.Contains(races, race => race.Name == "Golden Run São Paulo");
    }

    [Fact]
    public async Task Journey_ExposesCurrentKitStep()
    {
        var response = await _client.GetAsync("/api/me/journey");
        var body = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Kit em preparação", body);
    }

    private sealed record RaceSummaryContract(Guid Id, string Name);
}
