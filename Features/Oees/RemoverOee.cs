using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Oees;

/// <summary>Slice "Remover OEE": apaga um registro, ou 404 se não existir.</summary>
public static class RemoverOee
{
    public class Handler(AppDbContext db)
    {
        /// <returns>true se removeu; false se o id não existir.</returns>
        public async Task<bool> HandleAsync(Guid id, CancellationToken ct)
        {
            var oee = await db.Oees.FindAsync([id], ct);
            if (oee is null) return false;

            db.Oees.Remove(oee);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapDelete("/{id:guid}", async (Guid id, Handler handler, CancellationToken ct) =>
                await handler.HandleAsync(id, ct) ? Results.NoContent() : Results.NotFound())
            .WithName("RemoverOee")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
}
