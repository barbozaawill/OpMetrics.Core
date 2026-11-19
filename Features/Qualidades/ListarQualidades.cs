using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Qualidades;

/// <summary>Slice "Listar inspeções de qualidade": todas, da mais recente para a mais antiga.</summary>
public static class ListarQualidades
{
    public class Handler(AppDbContext db)
    {
        public async Task<List<QualidadeResposta>> HandleAsync(CancellationToken ct)
        {
            var qualidades = await db.Qualidades
                .AsNoTracking()
                .OrderByDescending(x => x.Data)
                .ToListAsync(ct);

            return qualidades.Select(QualidadeResposta.De).ToList();
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapGet("/", async (Handler handler, CancellationToken ct) =>
                Results.Ok(await handler.HandleAsync(ct)))
            .WithName("ListarQualidades")
            .Produces<List<QualidadeResposta>>();
}
