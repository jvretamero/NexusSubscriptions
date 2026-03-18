using NexusSubscriptions.Api.Infrasctructure.Database;

namespace NexusSubscriptions.Api.IntegrationTests;

public abstract class NexusSubscriptionsApiFixture(NexusSubscriptionsApiFactory factory) : IClassFixture<NexusSubscriptionsApiFactory>, IAsyncLifetime
{
    protected HttpClient? _client = null;
    protected HttpClient Client => _client ??= factory.CreateClient();

    protected ApiContext GetContext() => factory.GetContext();

    async Task IAsyncLifetime.InitializeAsync()
    {
        using var db = GetContext();

        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync(); // No migration, just the complete schema
    }

    Task IAsyncLifetime.DisposeAsync() => Task.CompletedTask;
}
