using Microsoft.EntityFrameworkCore;
using NexusSubscriptions.Api.Features.Plans;

namespace NexusSubscriptions.Api.Infrasctructure.Database;

public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
{
    public DbSet<Plan> Plans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiContext).Assembly);
    }
}
