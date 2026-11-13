using FluentValidation;

namespace OpMetrics.Core.Common;

/// <summary>
/// Filtro de endpoint que valida o corpo da requisição com o FluentValidation
/// antes de chegar no handler. Se houver erro, responde 400 com a lista de erros por campo.
///
/// O validator é buscado no RequestServices (e não no construtor) porque os validators
/// são registrados como Scoped: um por requisição.
/// </summary>
public class ValidationFilter<TRequest> : IEndpointFilter where TRequest : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<TRequest>().FirstOrDefault();
        var validator = context.HttpContext.RequestServices.GetService<IValidator<TRequest>>();

        if (request is null || validator is null)
            return await next(context);

        var resultado = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);

        if (!resultado.IsValid)
            return Results.ValidationProblem(resultado.ToDictionary());

        return await next(context);
    }
}

public static class ValidationFilterExtensions
{
    /// <summary>Atalho: <c>.ComValidacao&lt;MeuRequest&gt;()</c> no endpoint.</summary>
    public static RouteHandlerBuilder ComValidacao<TRequest>(this RouteHandlerBuilder builder) where TRequest : class =>
        builder.AddEndpointFilter<ValidationFilter<TRequest>>()
               .ProducesValidationProblem();
}
