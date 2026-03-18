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

        var addedPlan = await context.Plans.AddAsync(new Plan
        {
            Description = description,
            Price = 1m
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
        responseData.Should().BeEquivalentTo(new PlanDTO(planId, "Test plan", 1m));
    }

    [Fact]
    public async Task GetPlanById_Invalid_Id_Should_Return_400()
    {
        var response = await Client.GetAsync($"/api/plans/{9999}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
