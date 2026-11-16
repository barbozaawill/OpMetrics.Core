using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Oees;

/// <summary>Slice "Listar OEEs": todos os registros, do mais recente para o mais antigo.</summary>
public static class ListarOees
{
    public class Handler(AppDbContext db)
    {
        public async Task<List<OeeResposta>> HandleAsync(CancellationToken ct)
        {
            var oees = await db.Oees
                .AsNoTracking()
                .OrderByDescending(x => x.Data)
                .ToListAsync(ct);

            return oees.Select(OeeResposta.De).ToList();
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapGet("/", async (Handler handler, CancellationToken ct) =>
                Results.Ok(await handler.HandleAsync(ct)))
            .WithName("ListarOees")
            .Produces<List<OeeResposta>>();
}
