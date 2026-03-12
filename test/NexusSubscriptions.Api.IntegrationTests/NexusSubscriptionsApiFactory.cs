using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace NexusSubscriptions.Api.IntegrationTests;

public class NexusSubscriptionsApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public Task InitializeAsync()
    {
        //TODO initialize db connection
        return Task.CompletedTask;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            //TODO configure the db test connection
        });
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        //TODO dispose db connection
        return Task.CompletedTask;
    }
}
