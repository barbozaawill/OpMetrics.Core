using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Producoes;

/// <summary>Slice "Obter produção por id": devolve um registro ou 404.</summary>
public static class ObterProducaoPorId
{
    public class Handler(AppDbContext db)
    {
        public async Task<ProducaoResposta?> HandleAsync(Guid id, CancellationToken ct)
        {
            var producao = await db.Producoes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            return producao is null ? null : ProducaoResposta.De(producao);
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapGet("/{id:guid}", async (Guid id, Handler handler, CancellationToken ct) =>
            {
                var resposta = await handler.HandleAsync(id, ct);
                return resposta is null ? Results.NotFound() : Results.Ok(resposta);
            })
            .WithName("ObterProducaoPorId")
            .Produces<ProducaoResposta>()
            .Produces(StatusCodes.Status404NotFound);
}
