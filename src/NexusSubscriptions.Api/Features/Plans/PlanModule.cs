using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NexusSubscriptions.Api.Infrasctructure.Filters;
using NexusSubscriptions.Api.Infrasctructure.Handlers;

namespace NexusSubscriptions.Api.Features.Plans;

public static class PlanModule
{
    public static IServiceCollection AddPlanModule(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreatePlanRequest, Plan>, CreatePlanHandler>();
        services.AddScoped<IQueryHandler<GetAllPlansRequest, GetAllPlansResponse>, GetAllPlansHandler>();
        services.AddTransient<IValidator<CreatePlanRequest>, CreatePlanValidator>();

        return services;
    }

    public static void MapPlanModule(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/plans")
            .WithTags("Plans");

        group.MapPost("/", CreatePlan)
            .AddEndpointFilter<ValidationFilter<CreatePlanRequest>>();

        group.MapGet("/", GetAllPlans);
    }

    private static async Task<IResult> CreatePlan(
        [FromBody] CreatePlanRequest request,
        [FromServices] ICommandHandler<CreatePlanRequest, Plan> handler,
        CancellationToken ct)
    {
        var createdPlan = await handler.HandleAsync(request, ct);
        return TypedResults.Created("/api/plans/0", createdPlan);
    }

    private static async Task<IResult> GetAllPlans(
        [FromServices] IQueryHandler<GetAllPlansRequest, GetAllPlansResponse> handler,
        CancellationToken ct)
    {
        var response = await handler.HandleAsync(new GetAllPlansRequest(), ct);
        return TypedResults.Ok(response.Plans);
    }
}