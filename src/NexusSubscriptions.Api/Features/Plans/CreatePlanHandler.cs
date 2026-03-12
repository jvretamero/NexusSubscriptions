using FluentValidation;
using NexusSubscriptions.Api.Infrasctructure.Handlers;

namespace NexusSubscriptions.Api.Features.Plans;

public record CreatePlanRequest(
    string Description,
    decimal Price);

public class CreatePlanValidator : AbstractValidator<CreatePlanRequest>
{
    public CreatePlanValidator()
    {
    }
}

public class CreatePlanHandler : ICommandHandler<CreatePlanRequest, Plan>
{
    public Task<Plan> HandleAsync(CreatePlanRequest request, CancellationToken ct)
    {
        //TODO connect to database
        return Task.FromResult(new Plan
        {
            Id = 1,
            Description = "Test plan",
            Price = 10m
        });
    }
}
