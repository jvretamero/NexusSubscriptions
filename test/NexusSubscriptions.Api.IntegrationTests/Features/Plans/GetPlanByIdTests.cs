using System.Net.Http.Json;
using FluentAssertions;
using NexusSubscriptions.Api.Features.Plans;
using NexusSubscriptions.Api.Features.Plans.Domain;

namespace NexusSubscriptions.Api.IntegrationTests.Features.Plans;

[Collection("IntegrationTests")]
public class GetPlanByIdTests : NexusSubscriptionsApiFixture
{
    public GetPlanByIdTests(NexusSubscriptionsApiFactory factory) : base(factory)
    { }

    [Fact]
    public async Task GetPlanById_Returns200()
    {
        int planId = 0;

        using (var context = GetContext())
        {
            context.RemoveRange(context.Plans);

            var addedPlan = await context.Plans.AddAsync(new Plan
            {
                Description = "Test plan",
                Price = 1m
            });

            await context.SaveChangesAsync();

            planId = addedPlan.Entity.Id;
        }

        var response = await Client.GetAsync($"/api/plans/{planId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var responseData = await response.Content.ReadFromJsonAsync<PlanDTO>();

        responseData.Should().NotBeNull();
        responseData.Should().BeEquivalentTo(new PlanDTO(planId, "Test plan", 1m));
    }
}
