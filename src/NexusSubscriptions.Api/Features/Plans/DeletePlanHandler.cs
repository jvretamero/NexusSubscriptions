using Microsoft.EntityFrameworkCore;
using NexusSubscriptions.Api.Infrasctructure.Database;
using NexusSubscriptions.Api.Infrasctructure.Handlers;

namespace NexusSubscriptions.Api.Features.Plans;

public record DeletePlanRequest(int Id);

public record DeletePlanResponse(int? PlanId);

public class DeletePlanHandler(ApiContext context) : ICommandHandler<DeletePlanRequest, DeletePlanResponse>
{
    public async Task<DeletePlanResponse> HandleAsync(DeletePlanRequest request, CancellationToken ct)
    {
        var plan = await context.Plans
            .SingleOrDefaultAsync(plan => plan.Id == request.Id, ct);

        if (plan == null)
            return new DeletePlanResponse(PlanId: null);

        context.Plans.Remove(plan);

        await context.SaveChangesAsync(ct);

        return new DeletePlanResponse(plan.Id);
    }
}