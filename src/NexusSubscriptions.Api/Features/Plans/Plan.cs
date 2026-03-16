using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NexusSubscriptions.Api.Features.Plans;

public class Plan
{
    public int Id { get; set; }

    public required string Description { get; set; }

    public decimal Price { get; set; }
}

public class PlanTypeConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.ToTable("Plans")
            .HasKey(p => p.Id);

        builder.Property(p => p.Description)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(p => p.Price)
            .IsRequired();
    }
}