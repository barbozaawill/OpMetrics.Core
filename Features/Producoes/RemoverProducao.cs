using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Producoes;

/// <summary>Slice "Remover produção": apaga um registro, ou 404 se não existir.</summary>
public static class RemoverProducao
{
    public class Handler(AppDbContext db)
    {
        /// <returns>true se removeu; false se o id não existir.</returns>
        public async Task<bool> HandleAsync(Guid id, CancellationToken ct)
        {
            var producao = await db.Producoes.FindAsync([id], ct);
            if (producao is null) return false;

            db.Producoes.Remove(producao);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapDelete("/{id:guid}", async (Guid id, Handler handler, CancellationToken ct) =>
                await handler.HandleAsync(id, ct) ? Results.NoContent() : Results.NotFound())
            .WithName("RemoverProducao")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
}
