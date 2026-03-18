using System.Net.Http.Json;
using FluentAssertions;
using NexusSubscriptions.Api.Features.Plans;

namespace NexusSubscriptions.Api.IntegrationTests.Features.Plans;

[Collection("IntegrationTests")]
public class GetAllPlansTests : NexusSubscriptionsApiFixture
{
    public GetAllPlansTests(NexusSubscriptionsApiFactory factory) : base(factory)
    { }

    [Fact]
    public async Task GetAllPlans_Returns200()
    {
        using (var context = GetContext())
        {
            context.RemoveRange(context.Plans);

            await context.Plans.AddRangeAsync(
                new Plan { Description = "Test plan 1", Price = 1m },
                new Plan { Description = "Test plan 2", Price = 2m },
                new Plan { Description = "Test plan 3", Price = 3m }
            );

            await context.SaveChangesAsync();
        }

        var response = await Client.GetAsync("/api/plans");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var responseData = await response.Content.ReadFromJsonAsync<GetAllPlansResponse>();

        responseData.Should().NotBeNull();
        responseData.Plans.Should().HaveCount(3);

        responseData.Plans.Should().Contain(p => p.Description == "Test plan 1");
        responseData.Plans.Should().Contain(p => p.Description == "Test plan 2");
        responseData.Plans.Should().Contain(p => p.Description == "Test plan 3");
    }
}
