using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Qualidades;

/// <summary>Slice "Listar inspeções de qualidade por linha de produção".</summary>
public static class ListarQualidadesPorLinha
{
    public class Handler(AppDbContext db)
    {
        public async Task<List<QualidadeResposta>> HandleAsync(string linha, CancellationToken ct)
        {
            var qualidades = await db.Qualidades
                .AsNoTracking()
                .Where(x => x.Linha == linha)
                .OrderByDescending(x => x.Data)
                .ToListAsync(ct);

            return qualidades.Select(QualidadeResposta.De).ToList();
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapGet("/linha/{linha}", async (string linha, Handler handler, CancellationToken ct) =>
                Results.Ok(await handler.HandleAsync(linha, ct)))
            .WithName("ListarQualidadesPorLinha")
            .Produces<List<QualidadeResposta>>();
}
