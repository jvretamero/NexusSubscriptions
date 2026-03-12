using Microsoft.EntityFrameworkCore;
using NexusSubscriptions.Api.Features.Plans;
using NexusSubscriptions.Api.Infrasctructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
    .AddPlanModule();

builder.Services.AddDbContext<ApiContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("NexusSubscriptionsDb");
    options.UseSqlite(connectionString);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
    await context.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapPlanModule();

app.Run();