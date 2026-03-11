using FluentValidation;

namespace NexusSubscriptions.Api.Infrasctructure.Filters;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is null)
            return await next(context);

        var requestObj = context.Arguments.OfType<T>().FirstOrDefault();
        if (requestObj is null)
            return Results.Problem("Invalid request format.");

        var validationResult = await validator.ValidateAsync(requestObj, context.HttpContext.RequestAborted);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        return await next(context);
    }
}
