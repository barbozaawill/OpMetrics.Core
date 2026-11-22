using FluentValidation;
using OpMetrics.Core.Common;
using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Producoes;

/// <summary>Slice "Atualizar produção": substitui os dados de um registro existente.</summary>
public static class AtualizarProducao
{
    public record Request(
        string Linha,
        DateTime Data,
        string Turno,
        double MetaPecas,
        double PecasProduzidas) : IProducaoRequest;

    public class Validator : AbstractValidator<Request>
    {
        public Validator() => Include(new ProducaoRequestRegras());
    }

    public class Handler(AppDbContext db)
    {
        /// <returns>O registro atualizado, ou null se o id não existir.</returns>
        public async Task<ProducaoResposta?> HandleAsync(Guid id, Request request, CancellationToken ct)
        {
            var producao = await db.Producoes.FindAsync([id], ct);
            if (producao is null) return null;

            producao.Linha = request.Linha;
            producao.Data = request.Data;
            producao.Turno = request.Turno;
            producao.MetaPecas = request.MetaPecas;
            producao.PecasProduzidas = request.PecasProduzidas;
            producao.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync(ct);

            return ProducaoResposta.De(producao);
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapPut("/{id:guid}", async (Guid id, Request request, Handler handler, CancellationToken ct) =>
            {
                var resposta = await handler.HandleAsync(id, request, ct);
                return resposta is null ? Results.NotFound() : Results.Ok(resposta);
            })
            .ComValidacao<Request>()
            .WithName("AtualizarProducao")
            .Produces<ProducaoResposta>()
            .Produces(StatusCodes.Status404NotFound);
}
