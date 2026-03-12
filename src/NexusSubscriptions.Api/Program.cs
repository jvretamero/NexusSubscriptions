using NexusSubscriptions.Api.Features.Plans;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
    .AddPlanModule();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapPlanModule();

app.Run();