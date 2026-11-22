using FluentValidation;
using OpMetrics.Core.Common;
using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Oees;

/// <summary>Slice "Atualizar OEE": substitui os dados de um registro existente.</summary>
public static class AtualizarOee
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
        public Validator() => Include(new OeeRequestRegras());
    }

    public class Handler(AppDbContext db)
    {
        /// <returns>O registro atualizado, ou null se o id não existir.</returns>
        public async Task<OeeResposta?> HandleAsync(Guid id, Request request, CancellationToken ct)
        {
            // Aqui SEM AsNoTracking: o EF precisa acompanhar o objeto para saber
            // o que mudou e gerar o UPDATE no SaveChanges.
            var oee = await db.Oees.FindAsync([id], ct);
            if (oee is null) return null;

            oee.Linha = request.Linha;
            oee.Data = request.Data;
            oee.Turno = request.Turno;
            oee.TempoPlaneadoMinutos = request.TempoPlaneadoMinutos;
            oee.TempoRodandoMinutos = request.TempoRodandoMinutos;
            oee.CapacidadeIdealPecas = request.CapacidadeIdealPecas;
            oee.PecasProduzidas = request.PecasProduzidas;
            oee.PecasBoas = request.PecasBoas;
            oee.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync(ct);

            return OeeResposta.De(oee);
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapPut("/{id:guid}", async (Guid id, Request request, Handler handler, CancellationToken ct) =>
            {
                var resposta = await handler.HandleAsync(id, request, ct);
                return resposta is null ? Results.NotFound() : Results.Ok(resposta);
            })
            .ComValidacao<Request>()
            .WithName("AtualizarOee")
            .Produces<OeeResposta>()
            .Produces(StatusCodes.Status404NotFound);
}
