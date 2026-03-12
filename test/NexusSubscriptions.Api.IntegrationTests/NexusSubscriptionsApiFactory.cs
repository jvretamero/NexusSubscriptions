using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NexusSubscriptions.Api.Infrasctructure.Database;

namespace NexusSubscriptions.Api.IntegrationTests;

public class NexusSubscriptionsApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private DbConnection? dbConnection = null;

    public async Task InitializeAsync()
    {
        dbConnection = new SqliteConnection("DataSource=:memory:");
        await dbConnection.OpenAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApiContext>>();

            services.AddDbContext<ApiContext>(options =>
            {
                options.UseSqlite(dbConnection!);
            });

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
                context.Database.EnsureCreated();
            }
        });
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        if (dbConnection != null)
        {
            await dbConnection.CloseAsync();
            await dbConnection.DisposeAsync();
        }
    }
}
