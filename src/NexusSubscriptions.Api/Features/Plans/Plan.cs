namespace NexusSubscriptions.Api.Features.Plans;

public class Plan
{
    public int Id { get; set; }

    public required string Description { get; set; }

    public decimal Price { get; set; }
}
