using Microsoft.AspNetCore.Mvc;
using NexusSubscriptions.Api.Infrasctructure.Handlers;

namespace NexusSubscriptions.Api.Features.Plans;

public static class PlanModule
{
    public static IServiceCollection AddPlanModule(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreatePlanRequest, Plan>, CreatePlanHandler>();
        
        return services;
    }

    public static void MapPlanModule(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/plans")
            .WithTags("Plans");

        group.MapPost("/", CreatePlan);
    }

    private static async Task<IResult> CreatePlan(
        [FromBody] CreatePlanRequest request,
        [FromServices] ICommandHandler<CreatePlanRequest, Plan> handler,
        CancellationToken ct)
    {
        var createdPlan = await handler.HandleAsync(request, ct);
        return TypedResults.Created("/api/plans/0", createdPlan);
    }
}