using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Producoes;

/// <summary>Slice "Listar produções por linha de produção".</summary>
public static class ListarProducoesPorLinha
{
    public class Handler(AppDbContext db)
    {
        public async Task<List<ProducaoResposta>> HandleAsync(string linha, CancellationToken ct)
        {
            var producoes = await db.Producoes
                .AsNoTracking()
                .Where(x => x.Linha == linha)
                .OrderByDescending(x => x.Data)
                .ToListAsync(ct);

            return producoes.Select(ProducaoResposta.De).ToList();
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapGet("/linha/{linha}", async (string linha, Handler handler, CancellationToken ct) =>
                Results.Ok(await handler.HandleAsync(linha, ct)))
            .WithName("ListarProducoesPorLinha")
            .Produces<List<ProducaoResposta>>();
}
