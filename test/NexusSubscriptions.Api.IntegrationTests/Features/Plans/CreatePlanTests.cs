using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NexusSubscriptions.Api.Features.Plans;

namespace NexusSubscriptions.Api.IntegrationTests.Features.Plans;

[Collection("IntegrationTests")]
public class CreatePlanTests(NexusSubscriptionsApiFactory factory) : IClassFixture<NexusSubscriptionsApiFactory>
{
    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task CreatePlan_Returns201AndSavesPlanToDatabase()
    {
        var planDescription = "Test plan";
        var planPrice = 10m;

        var request = new CreatePlanRequest(planDescription, planPrice);

        var response = await client.PostAsJsonAsync("/api/plans", request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var createdPlan = await response.Content.ReadFromJsonAsync<Plan>();
        createdPlan.Should().NotBeNull();
        createdPlan!.Id.Should().BeGreaterThan(0, "Plan id not positive");
        createdPlan!.Description.Should().NotBeNullOrEmpty().And.BeEquivalentTo(planDescription, "Invalid plan description");
        createdPlan!.Price.Should().BeApproximately(planPrice, 0m, "Invalid plan price");
    }

    [Fact]
    public async Task CreatePlan_ReturnsBadRequest()
    {
        var request = new CreatePlanRequest("This is a very long plan description", -1m);

        var response = await client.PostAsJsonAsync("/api/plans", request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        using var db = factory.GetContext();
        var planCount = await db.Plans.CountAsync();
        planCount.Should().Be(0);
    }
}
