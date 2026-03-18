namespace NexusSubscriptions.Api.Infrasctructure.Filters;

public static class ValidationFilterExtensions
{
    public static RouteHandlerBuilder AddValidationFilter<T>(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<ValidationFilter<T>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesValidationProblem();
    }
}
