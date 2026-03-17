using FluentValidation;
using NexusSubscriptions.Api.Infrasctructure.Database;
using NexusSubscriptions.Api.Infrasctructure.Handlers;

namespace NexusSubscriptions.Api.Features.Plans;

public record CreatePlanRequest(
    string Description,
    decimal Price);

public class CreatePlanValidator : AbstractValidator<CreatePlanRequest>
{
    public CreatePlanValidator()
    {
        RuleFor(model => model.Description)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(model => model.Price)
            .NotNull()
            .GreaterThan(0m);
    }
}

public class CreatePlanHandler(ApiContext context) : ICommandHandler<CreatePlanRequest, Plan>
{
    public async Task<Plan> HandleAsync(CreatePlanRequest request, CancellationToken ct)
    {
        var newPlan = new Plan
        {
            Description = request.Description,
            Price = request.Price
        };

        var createdPlan = await context.Plans.AddAsync(newPlan, ct);

        await context.SaveChangesAsync(ct);

        return createdPlan.Entity;
    }
}
