using Microsoft.EntityFrameworkCore;
using NexusSubscriptions.Api.Infrasctructure.Database;
using NexusSubscriptions.Api.Infrasctructure.Handlers;

namespace NexusSubscriptions.Api.Features.Plans;

public record UpdatePlanDTO(string Description, decimal Price);

public record UpdatePlanRequest(int Id, string Description, decimal Price);

public record UpdatePlanResponse(PlanDTO? Plan);

public class UpdatePlanHandler(ApiContext context) : ICommandHandler<UpdatePlanRequest, UpdatePlanResponse>
{
    public async Task<UpdatePlanResponse> HandleAsync(UpdatePlanRequest request, CancellationToken ct)
    {
        var plan = await context.Plans
            .SingleOrDefaultAsync(plan => plan.Id == request.Id, ct);

        if (plan == null)
            return new UpdatePlanResponse(Plan: null);

        plan.Description = request.Description;
        plan.Price = request.Price;

        await context.SaveChangesAsync(ct);

        return new UpdatePlanResponse(new PlanDTO(plan.Id, plan.Description, plan.Price));
    }
}
