using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NexusSubscriptions.Api.Infrasctructure.Database;
using NexusSubscriptions.Api.Infrasctructure.Handlers;

namespace NexusSubscriptions.Api.Features.Plans;

public record UpdatePlanDTO(string Description, decimal Price);

public record UpdatePlanRequest(int Id, string Description, decimal Price);

public record UpdatePlanResponse(PlanDTO? Plan);

public class UpdatePlanValidator : AbstractValidator<UpdatePlanDTO>
{
    public UpdatePlanValidator()
    {
        RuleFor(model => model.Description)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(model => model.Price)
            .NotNull()
            .GreaterThan(0m);
    }
}

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
        plan.UpdatedAt = DateTime.Now;

        await context.SaveChangesAsync(ct);

        return new UpdatePlanResponse(new PlanDTO(plan.Id, plan.Description, plan.Price, plan.CreatedAt, plan.UpdatedAt));
    }
}
