using System.Net.Http.Json;
using FluentAssertions;
using NexusSubscriptions.Api.Features.Plans;

namespace NexusSubscriptions.Api.IntegrationTests.Features.Plans;

[Collection("IntegrationTests")]
public class GetAllPlansTests(NexusSubscriptionsApiFactory factory) : IClassFixture<NexusSubscriptionsApiFactory>
{
    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task GetAllPlans_Returns200()
    {
        using (var context = factory.GetContext())
        {
            context.RemoveRange(context.Plans);

            await context.Plans.AddRangeAsync(
                new Plan { Description = "Test plan 1", Price = 1m },
                new Plan { Description = "Test plan 2", Price = 2m },
                new Plan { Description = "Test plan 3", Price = 3m }
            );

            await context.SaveChangesAsync();
        }

        var response = await client.GetAsync("/api/plans");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var responseData = await response.Content.ReadFromJsonAsync<List<Plan>>();

        responseData.Should().NotBeNull();
        responseData!.Should().HaveCount(3);

        responseData.Should().Contain(p => p.Description == "Test plan 1");
        responseData.Should().Contain(p => p.Description == "Test plan 2");
        responseData.Should().Contain(p => p.Description == "Test plan 3");
    }
}
