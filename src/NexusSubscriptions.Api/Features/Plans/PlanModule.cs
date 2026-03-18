using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NexusSubscriptions.Api.Infrasctructure.Filters;
using NexusSubscriptions.Api.Infrasctructure.Handlers;

namespace NexusSubscriptions.Api.Features.Plans;

public static class PlanModule
{
    public static IServiceCollection AddPlanModule(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreatePlanRequest, PlanDTO>, CreatePlanHandler>();
        services.AddScoped<IQueryHandler<GetAllPlansRequest, GetAllPlansResponse>, GetAllPlansHandler>();
        services.AddTransient<IValidator<CreatePlanRequest>, CreatePlanValidator>();

        return services;
    }

    public static void MapPlanModule(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/plans")
            .WithTags("Plans");

        group.MapPost("/", CreatePlan)
            .WithName("CreatePlan")
            .AddValidationFilter<CreatePlanRequest>()
            .Produces<PlanDTO>();

        group.MapGet("/", GetAllPlans)
            .WithName("GetAllPlans")
            .Produces<GetAllPlansResponse>();
    }

    private static async Task<IResult> CreatePlan(
        [FromBody] CreatePlanRequest request,
        [FromServices] ICommandHandler<CreatePlanRequest, PlanDTO> handler,
        CancellationToken ct)
    {
        var response = await handler.HandleAsync(request, ct);
        return TypedResults.Created($"/api/plans/{response.Id}", response);
    }

    private static async Task<IResult> GetAllPlans(
        [FromServices] IQueryHandler<GetAllPlansRequest, GetAllPlansResponse> handler,
        CancellationToken ct)
    {
        var response = await handler.HandleAsync(new GetAllPlansRequest(), ct);
        return TypedResults.Ok(response);
    }
}