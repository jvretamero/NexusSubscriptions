using Microsoft.EntityFrameworkCore;
using NexusSubscriptions.Api.Infrasctructure.Database;
using NexusSubscriptions.Api.Infrasctructure.Handlers;

namespace NexusSubscriptions.Api.Features.Plans;

public record GetPlanByIdRequest(int Id);

public record GetPlanByIdResponse(PlanDTO? Plan);

public class GetPlanByIdHandler(ApiContext context) : IQueryHandler<GetPlanByIdRequest, GetPlanByIdResponse>
{
    public async Task<GetPlanByIdResponse> HandleAsync(GetPlanByIdRequest request, CancellationToken ct)
    {
        var plan = await context.Plans
            .AsNoTracking()
            .Where(plan => plan.Id == request.Id)
            .Select(plan => new PlanDTO(plan.Id, plan.Description, plan.Price, plan.CreatedAt, plan.UpdatedAt))
            .SingleOrDefaultAsync(ct);

        return new GetPlanByIdResponse(plan);
    }
}
