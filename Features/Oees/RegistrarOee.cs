using FluentValidation;
using OpMetrics.Core.Common;
using OpMetrics.Core.Data;
using OpMetrics.Core.DTOs.Requests;
using OpMetrics.Core.Entities;
using OpMetrics.Core.Validators;

namespace OpMetrics.Core.Features.Oees;

/// <summary>
/// Slice "Registrar OEE": tudo o que essa operação precisa fica neste arquivo
/// (request, validação, regra de negócio e rota HTTP).
/// </summary>
public static class RegistrarOee
{
    public record Request(
        string Linha,
        DateTime Data,
        string Turno,
        double TempoPlaneadoMinutos,
        double TempoRodandoMinutos,
        double CapacidadeIdealPecas,
        double PecasProduzidas,
        double PecasBoas) : IOeeRequest;

    public class Validator : AbstractValidator<Request>
    {
        // As regras ficam em um lugar só (OeeRequestRegras) e são reaproveitadas
        // pelo registro e pela atualização.
        public Validator() => Include(new OeeRequestRegras());
    }

    public class Handler(AppDbContext db)
    {
        public async Task<OeeResposta> HandleAsync(Request request, CancellationToken ct)
        {
            var oee = new Oee
            {
                Linha = request.Linha,
                Data = request.Data,
                Turno = request.Turno,
                TempoPlaneadoMinutos = request.TempoPlaneadoMinutos,
                TempoRodandoMinutos = request.TempoRodandoMinutos,
                CapacidadeIdealPecas = request.CapacidadeIdealPecas,
                PecasProduzidas = request.PecasProduzidas,
                PecasBoas = request.PecasBoas
            };

            db.Oees.Add(oee);
            await db.SaveChangesAsync(ct);

            return OeeResposta.De(oee);
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapPost("/", async (Request request, Handler handler, CancellationToken ct) =>
            {
                var resposta = await handler.HandleAsync(request, ct);
                return Results.Created($"/api/oee/{resposta.Id}", resposta);
            })
            .ComValidacao<Request>()
            .WithName("RegistrarOee")
            .Produces<OeeResposta>(StatusCodes.Status201Created);
}
