namespace NexusSubscriptions.Api.Features.Plans;

public record PlanDTO(
    int Id,
    string Description,
    decimal Price);