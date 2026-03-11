using FluentValidation;
using NexusSubscriptions.Api.Infrasctructure.Handlers;

namespace NexusSubscriptions.Api.Features.Plans;

public record CreatePlanRequest;

public record CreatePlanResponse;

public class CreatePlanValidator : AbstractValidator<CreatePlanRequest>
{
    public CreatePlanValidator()
    {
    }
}

public class CreatePlanHandler : ICommandHandler<CreatePlanRequest, CreatePlanResponse>
{
    public Task<CreatePlanResponse> HandleAsync(CreatePlanRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
