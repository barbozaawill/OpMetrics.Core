using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Producoes;

/// <summary>Slice "Listar produções": todos os registros, do mais recente para o mais antigo.</summary>
public static class ListarProducoes
{
    public class Handler(AppDbContext db)
    {
        public async Task<List<ProducaoResposta>> HandleAsync(CancellationToken ct)
        {
            var producoes = await db.Producoes
                .AsNoTracking()
                .OrderByDescending(x => x.Data)
                .ToListAsync(ct);

            return producoes.Select(ProducaoResposta.De).ToList();
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapGet("/", async (Handler handler, CancellationToken ct) =>
                Results.Ok(await handler.HandleAsync(ct)))
            .WithName("ListarProducoes")
            .Produces<List<ProducaoResposta>>();
}
