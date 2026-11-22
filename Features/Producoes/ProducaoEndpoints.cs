namespace OpMetrics.Core.Features.Producoes;

/// <summary>
/// Ponto único de registro dos slices de Produção: serviços (handlers) e rotas.
/// O Program.cs só chama AddProducao() e MapProducao().
/// </summary>
public static class ProducaoEndpoints
{
    public static IServiceCollection AddProducao(this IServiceCollection services)
    {
        services.AddScoped<RegistrarProducao.Handler>();
        services.AddScoped<ObterProducaoPorId.Handler>();
        services.AddScoped<ListarProducoes.Handler>();
        services.AddScoped<ListarProducoesPorLinha.Handler>();
        services.AddScoped<AtualizarProducao.Handler>();
        services.AddScoped<RemoverProducao.Handler>();
        return services;
    }

    public static IEndpointRouteBuilder MapProducao(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/producao").WithTags("Produção");

        RegistrarProducao.Map(grupo);
        ObterProducaoPorId.Map(grupo);
        ListarProducoes.Map(grupo);
        ListarProducoesPorLinha.Map(grupo);
        AtualizarProducao.Map(grupo);
        RemoverProducao.Map(grupo);

        return app;
    }
}
