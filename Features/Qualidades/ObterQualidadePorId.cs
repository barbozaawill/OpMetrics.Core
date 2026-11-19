using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Qualidades;

/// <summary>Slice "Obter inspeção de qualidade por id": devolve um registro ou 404.</summary>
public static class ObterQualidadePorId
{
    public class Handler(AppDbContext db)
    {
        public async Task<QualidadeResposta?> HandleAsync(Guid id, CancellationToken ct)
        {
            var qualidade = await db.Qualidades
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            return qualidade is null ? null : QualidadeResposta.De(qualidade);
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapGet("/{id:guid}", async (Guid id, Handler handler, CancellationToken ct) =>
            {
                var resposta = await handler.HandleAsync(id, ct);
                return resposta is null ? Results.NotFound() : Results.Ok(resposta);
            })
            .WithName("ObterQualidadePorId")
            .Produces<QualidadeResposta>()
            .Produces(StatusCodes.Status404NotFound);
}
