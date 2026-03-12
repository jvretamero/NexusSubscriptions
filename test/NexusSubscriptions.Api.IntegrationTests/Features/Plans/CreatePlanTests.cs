using System.Net.Http.Json;
using FluentAssertions;
using NexusSubscriptions.Api.Features.Plans;

namespace NexusSubscriptions.Api.IntegrationTests.Features.Plans;

[Collection("IntegrationTests")]
public class CreatePlanTests(NexusSubscriptionsApiFactory factory) : IClassFixture<NexusSubscriptionsApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreatePlan_Returns201AndSavesPlanToDatabase()
    {
        var request = new CreatePlanRequest(Description: "Test plan", Price: 10m);

        var response = await _client.PostAsJsonAsync("/api/plans", request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var createdPlan = await response.Content.ReadFromJsonAsync<Plan>();
        createdPlan.Should().NotBeNull();
        createdPlan!.Id.Should().BeGreaterThan(0);
    }
}
