namespace OpMetrics.Core.Features.Qualidades;

/// <summary>
/// Ponto único de registro dos slices de Qualidade: serviços (handlers) e rotas.
/// O Program.cs só chama AddQualidade() e MapQualidade().
/// </summary>
public static class QualidadeEndpoints
{
    public static IServiceCollection AddQualidade(this IServiceCollection services)
    {
        services.AddScoped<RegistrarQualidade.Handler>();
        services.AddScoped<ObterQualidadePorId.Handler>();
        services.AddScoped<ListarQualidades.Handler>();
        services.AddScoped<ListarQualidadesPorLinha.Handler>();
        services.AddScoped<AtualizarQualidade.Handler>();
        services.AddScoped<RemoverQualidade.Handler>();
        return services;
    }

    public static IEndpointRouteBuilder MapQualidade(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/qualidade").WithTags("Qualidade");

        RegistrarQualidade.Map(grupo);
        ObterQualidadePorId.Map(grupo);
        ListarQualidades.Map(grupo);
        ListarQualidadesPorLinha.Map(grupo);
        AtualizarQualidade.Map(grupo);
        RemoverQualidade.Map(grupo);

        return app;
    }
}
