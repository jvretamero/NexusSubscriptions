namespace NexusSubscriptions.Api.Infrasctructure.Handlers;

public interface IQueryHandler<in TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken ct);
}
