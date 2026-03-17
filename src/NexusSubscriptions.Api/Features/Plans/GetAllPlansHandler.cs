using Microsoft.EntityFrameworkCore;
using NexusSubscriptions.Api.Infrasctructure.Database;
using NexusSubscriptions.Api.Infrasctructure.Handlers;

namespace NexusSubscriptions.Api.Features.Plans;

public record GetAllPlansRequest;

public record GetAllPlansResponse(List<Plan> Plans);

public class GetAllPlansHandler(ApiContext context) : IQueryHandler<GetAllPlansRequest, GetAllPlansResponse>
{
    public async Task<GetAllPlansResponse> HandleAsync(GetAllPlansRequest request, CancellationToken ct)
    {
        var plans = await context.Plans
            .OrderBy(plan => plan.Id)
            .AsNoTracking()
            .ToListAsync(ct);
            
        return new GetAllPlansResponse(plans);
    }
}