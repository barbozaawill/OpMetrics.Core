namespace OpMetrics.Core.Features.Oees;

/// <summary>
/// Ponto único de registro dos slices de OEE: serviços (handlers) e rotas.
/// O Program.cs só chama AddOee() e MapOee().
/// </summary>
public static class OeeEndpoints
{
    public static IServiceCollection AddOee(this IServiceCollection services)
    {
        services.AddScoped<RegistrarOee.Handler>();
        return services;
    }

    public static IEndpointRouteBuilder MapOee(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/oee").WithTags("OEE");

        RegistrarOee.Map(grupo);

        return app;
    }
}
