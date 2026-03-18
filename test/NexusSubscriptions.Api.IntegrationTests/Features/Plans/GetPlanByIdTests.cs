using System.Net.Http.Json;
using FluentAssertions;
using NexusSubscriptions.Api.Features.Plans;
using NexusSubscriptions.Api.Features.Plans.Domain;

namespace NexusSubscriptions.Api.IntegrationTests.Features.Plans;

[Collection("IntegrationTests")]
public class GetPlanByIdTests : NexusSubscriptionsApiFixture
{
    public GetPlanByIdTests(NexusSubscriptionsApiFactory factory) : base(factory)
    {
        using var context = GetContext();

        context.RemoveRange(context.Plans);

        context.SaveChanges();
    }

    private async Task<Plan> CreatePlan(string description)
    {
        using var context = GetContext();

        var now = DateTime.Now;
        var addedPlan = await context.Plans.AddAsync(new Plan
        {
            Description = description,
            Price = 1m,
            CreatedAt = now,
            UpdatedAt = now
        });

        await context.SaveChangesAsync();

        return addedPlan.Entity;
    }

    [Fact]
    public async Task GetPlanById_Valid_Id_Should_Return_200()
    {
        int planId = (await CreatePlan("Test plan")).Id;

        var response = await Client.GetAsync($"/api/plans/{planId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var responseData = await response.Content.ReadFromJsonAsync<PlanDTO>();

        responseData.Should().NotBeNull();
        responseData.Id.Should().Be(planId);
        responseData.Description.Should().Be("Test plan");
        responseData.Price.Should().Be(1m);
        responseData.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        responseData.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task GetPlanById_Invalid_Id_Should_Return_400()
    {
        var response = await Client.GetAsync($"/api/plans/{9999}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
