using Carter;

namespace NexusSubscriptions.Api.Features.Plans;

public class PlanModule : ICarterModule
{
    void ICarterModule.AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/plans")
            .WithTags("Plans");
    }
}