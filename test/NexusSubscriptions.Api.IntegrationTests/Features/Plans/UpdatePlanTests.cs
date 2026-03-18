using System.Net.Http.Json;
using FluentAssertions;
using NexusSubscriptions.Api.Features.Plans;
using NexusSubscriptions.Api.Features.Plans.Domain;

namespace NexusSubscriptions.Api.IntegrationTests.Features.Plans;

[Collection("IntegrationTests")]
public class UpdatePlanTests : NexusSubscriptionsApiFixture
{
    public UpdatePlanTests(NexusSubscriptionsApiFactory factory) : base(factory)
    {
        using var context = GetContext();

        context.RemoveRange(context.Plans);

        context.SaveChanges();
    }

    private async Task<Plan> CreatePlan(string description)
    {
        using var context = GetContext();

        var addedPlan = await context.Plans.AddAsync(new Plan
        {
            Description = description,
            Price = 1m
        });

        await context.SaveChangesAsync();

        return addedPlan.Entity;
    }

    [Fact]
    public async Task UpdatePlan_Valid_Id_Should_Return_200()
    {
        int planId = (await CreatePlan("Test plan")).Id;

        var requestBody = new UpdatePlanDTO("New description", 5m);
        var response = await Client.PutAsJsonAsync($"/api/plans/{planId}", requestBody);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var responseData = await response.Content.ReadFromJsonAsync<PlanDTO>();

        responseData.Should().NotBeNull("Plan not returned");
        responseData.Description.Should().BeEquivalentTo("New description", "Description not updated");
        responseData.Price.Should().BeApproximately(5m, 0m, "Price not updated");
    }

    [Fact]
    public async Task UpdatePlan_Invalid_Id_Should_Return_400()
    {
        var requestBody = new UpdatePlanDTO("New description", 5m);
        var response = await Client.PutAsJsonAsync($"/api/plans/{9999}", requestBody);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
