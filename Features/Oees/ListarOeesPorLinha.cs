using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Oees;

/// <summary>Slice "Listar OEEs por linha de produção".</summary>
public static class ListarOeesPorLinha
{
    public class Handler(AppDbContext db)
    {
        public async Task<List<OeeResposta>> HandleAsync(string linha, CancellationToken ct)
        {
            var oees = await db.Oees
                .AsNoTracking()
                .Where(x => x.Linha == linha)
                .OrderByDescending(x => x.Data)
                .ToListAsync(ct);

            return oees.Select(OeeResposta.De).ToList();
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapGet("/linha/{linha}", async (string linha, Handler handler, CancellationToken ct) =>
                Results.Ok(await handler.HandleAsync(linha, ct)))
            .WithName("ListarOeesPorLinha")
            .Produces<List<OeeResposta>>();
}
