using Microsoft.EntityFrameworkCore;
using NexusSubscriptions.Api.Infrasctructure.Database;
using NexusSubscriptions.Api.Infrasctructure.Handlers;

namespace NexusSubscriptions.Api.Features.Plans;

public record GetAllPlansRequest;

public record GetAllPlansResponse(List<PlanDTO> Plans);

public class GetAllPlansHandler(ApiContext context) : IQueryHandler<GetAllPlansRequest, GetAllPlansResponse>
{
    public async Task<GetAllPlansResponse> HandleAsync(GetAllPlansRequest request, CancellationToken ct)
    {
        var plans = await context.Plans
            .AsNoTracking()
            .OrderBy(plan => plan.Id)
            .Select(plan => new PlanDTO(plan.Id, plan.Description, plan.Price))
            .ToListAsync(ct);

        return new GetAllPlansResponse(plans);
    }
}