namespace NexusSubscriptions.Api.Infrasctructure.Handlers;

public interface ICommandHandler<in TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken ct);
}
