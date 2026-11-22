using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Oees;

/// <summary>Slice "Obter OEE por id": devolve um registro ou 404.</summary>
public static class ObterOeePorId
{
    public class Handler(AppDbContext db)
    {
        public async Task<OeeResposta?> HandleAsync(Guid id, CancellationToken ct)
        {
            // AsNoTracking: só vamos ler, então o EF não precisa acompanhar mudanças no objeto.
            var oee = await db.Oees
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            return oee is null ? null : OeeResposta.De(oee);
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapGet("/{id:guid}", async (Guid id, Handler handler, CancellationToken ct) =>
            {
                var resposta = await handler.HandleAsync(id, ct);
                return resposta is null ? Results.NotFound() : Results.Ok(resposta);
            })
            .WithName("ObterOeePorId")
            .Produces<OeeResposta>()
            .Produces(StatusCodes.Status404NotFound);
}
