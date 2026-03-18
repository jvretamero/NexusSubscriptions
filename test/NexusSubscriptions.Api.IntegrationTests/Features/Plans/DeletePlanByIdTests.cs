using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NexusSubscriptions.Api.Features.Plans;
using NexusSubscriptions.Api.Features.Plans.Domain;

namespace NexusSubscriptions.Api.IntegrationTests.Features.Plans;

[Collection("IntegrationTests")]
public class DeletePlanByIdTests : NexusSubscriptionsApiFixture
{
    public DeletePlanByIdTests(NexusSubscriptionsApiFactory factory) : base(factory)
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

    private async Task<int> GetPlanCountAsync()
    {
        using var context = GetContext();
        return await context.Plans.CountAsync();
    }

    [Fact]
    public async Task DeletePlanById_Valid_Id_Should_Return_204()
    {
        int planId = (await CreatePlan("Test plan")).Id;

        var response = await Client.DeleteAsync($"/api/plans/{planId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        int planCount = await GetPlanCountAsync();
        planCount.Should().Be(0, "Plan not deleted");
    }

    [Fact]
    public async Task DeletePlanById_Invalid_Id_Should_Return_400()
    {
        var response = await Client.DeleteAsync($"/api/plans/{9999}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
