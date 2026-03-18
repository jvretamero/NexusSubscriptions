using FluentValidation;
using NexusSubscriptions.Api.Features.Plans.Domain;
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

public class CreatePlanHandler(ApiContext context) : ICommandHandler<CreatePlanRequest, PlanDTO>
{
    public async Task<PlanDTO> HandleAsync(CreatePlanRequest request, CancellationToken ct)
    {
        var newPlan = new Plan
        {
            Description = request.Description,
            Price = request.Price
        };

        await context.Plans.AddAsync(newPlan, ct);
        await context.SaveChangesAsync(ct);

        return new PlanDTO(newPlan.Id, newPlan.Description, newPlan.Price);
    }
}
