using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Qualidades;

/// <summary>Slice "Remover inspeção de qualidade": apaga um registro, ou 404 se não existir.</summary>
public static class RemoverQualidade
{
    public class Handler(AppDbContext db)
    {
        /// <returns>true se removeu; false se o id não existir.</returns>
        public async Task<bool> HandleAsync(Guid id, CancellationToken ct)
        {
            var qualidade = await db.Qualidades.FindAsync([id], ct);
            if (qualidade is null) return false;

            db.Qualidades.Remove(qualidade);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapDelete("/{id:guid}", async (Guid id, Handler handler, CancellationToken ct) =>
                await handler.HandleAsync(id, ct) ? Results.NoContent() : Results.NotFound())
            .WithName("RemoverQualidade")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
}
