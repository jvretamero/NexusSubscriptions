using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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

    private async Task<Plan> CreatePlan(string description, DateTime createdAt)
    {
        using var context = GetContext();

        var now = DateTime.Now;
        var addedPlan = await context.Plans.AddAsync(new Plan
        {
            Description = description,
            Price = 1m,
            CreatedAt = createdAt,
            UpdatedAt = now
        });

        await context.SaveChangesAsync();

        return addedPlan.Entity;
    }

    [Fact]
    public async Task UpdatePlan_Valid_Id_Should_Return_200()
    {
        int planId = (await CreatePlan("Test plan", DateTime.Today)).Id;

        var requestBody = new UpdatePlanDTO("New description", 5m);
        var response = await Client.PutAsJsonAsync($"/api/plans/{planId}", requestBody);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var responseData = await response.Content.ReadFromJsonAsync<PlanDTO>();

        responseData.Should().NotBeNull("Plan not returned");
        responseData.Id.Should().Be(planId);
        responseData.Description.Should().Be("New description");
        responseData.Price.Should().Be(5m);
        responseData.CreatedAt.Should().BeCloseTo(DateTime.Today, TimeSpan.FromSeconds(0));
        responseData.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task UpdatePlan_Invalid_Id_Should_Return_400()
    {
        var requestBody = new UpdatePlanDTO("New description", 5m);
        var response = await Client.PutAsJsonAsync($"/api/plans/{9999}", requestBody);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdatePlan_Invalid_Data_ReturnsBadRequest()
    {
        int planId = (await CreatePlan("Test plan", DateTime.Today)).Id;

        var requestBody = new UpdatePlanDTO("This is a very long plan description", 5m);
        var response = await Client.PutAsJsonAsync($"/api/plans/{planId}", requestBody);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        using var context = GetContext();
        var plan = await context.Plans.SingleAsync(plan => plan.Id == planId);
        plan.Description.Should().Be("Test plan");
    }
}
