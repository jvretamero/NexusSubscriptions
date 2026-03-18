using Microsoft.EntityFrameworkCore;
using NexusSubscriptions.Api.Infrasctructure.Database;
using NexusSubscriptions.Api.Infrasctructure.Handlers;

namespace NexusSubscriptions.Api.Features.Plans;

public record DeletePlanByIdRequest(int Id);

public record DeletePlanByIdResponse(int? PlanId);

public class DeletePlanByIdHandler(ApiContext context) : ICommandHandler<DeletePlanByIdRequest, DeletePlanByIdResponse>
{
    public async Task<DeletePlanByIdResponse> HandleAsync(DeletePlanByIdRequest request, CancellationToken ct)
    {
        var plan = await context.Plans
            .SingleOrDefaultAsync(plan => plan.Id == request.Id, ct);

        if (plan == null)
            return new DeletePlanByIdResponse(PlanId: null);

        context.Plans.Remove(plan);

        await context.SaveChangesAsync(ct);

        return new DeletePlanByIdResponse(plan.Id);
    }
}