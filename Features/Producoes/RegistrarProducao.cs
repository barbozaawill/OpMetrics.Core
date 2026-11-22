using FluentValidation;
using OpMetrics.Core.Common;
using OpMetrics.Core.Data;
using OpMetrics.Core.Entities;

namespace OpMetrics.Core.Features.Producoes;

/// <summary>Slice "Registrar produção": request, validação, regra e rota num lugar só.</summary>
public static class RegistrarProducao
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
        public async Task<ProducaoResposta> HandleAsync(Request request, CancellationToken ct)
        {
            var producao = new Producao
            {
                Linha = request.Linha,
                Data = request.Data,
                Turno = request.Turno,
                MetaPecas = request.MetaPecas,
                PecasProduzidas = request.PecasProduzidas
            };

            db.Producoes.Add(producao);
            await db.SaveChangesAsync(ct);

            return ProducaoResposta.De(producao);
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapPost("/", async (Request request, Handler handler, CancellationToken ct) =>
            {
                var resposta = await handler.HandleAsync(request, ct);
                return Results.CreatedAtRoute("ObterProducaoPorId", new { id = resposta.Id }, resposta);
            })
            .ComValidacao<Request>()
            .WithName("RegistrarProducao")
            .Produces<ProducaoResposta>(StatusCodes.Status201Created);
}
