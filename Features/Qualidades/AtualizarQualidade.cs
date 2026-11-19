using FluentValidation;
using OpMetrics.Core.Common;
using OpMetrics.Core.Data;

namespace OpMetrics.Core.Features.Qualidades;

/// <summary>Slice "Atualizar inspeção de qualidade": substitui os dados de um registro existente.</summary>
public static class AtualizarQualidade
{
    public record Request(
        string Linha,
        DateTime Data,
        string Turno,
        int TotalInspecionado,
        int TotalDefeitos,
        int TotalRejeitos) : IQualidadeRequest;

    public class Validator : AbstractValidator<Request>
    {
        public Validator() => Include(new QualidadeRequestRegras());
    }

    public class Handler(AppDbContext db)
    {
        /// <returns>O registro atualizado, ou null se o id não existir.</returns>
        public async Task<QualidadeResposta?> HandleAsync(Guid id, Request request, CancellationToken ct)
        {
            var qualidade = await db.Qualidades.FindAsync([id], ct);
            if (qualidade is null) return null;

            qualidade.Linha = request.Linha;
            qualidade.Data = request.Data;
            qualidade.Turno = request.Turno;
            qualidade.TotalInspecionado = request.TotalInspecionado;
            qualidade.TotalDefeitos = request.TotalDefeitos;
            qualidade.TotalRejeitos = request.TotalRejeitos;
            qualidade.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync(ct);

            return QualidadeResposta.De(qualidade);
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapPut("/{id:guid}", async (Guid id, Request request, Handler handler, CancellationToken ct) =>
            {
                var resposta = await handler.HandleAsync(id, request, ct);
                return resposta is null ? Results.NotFound() : Results.Ok(resposta);
            })
            .ComValidacao<Request>()
            .WithName("AtualizarQualidade")
            .Produces<QualidadeResposta>()
            .Produces(StatusCodes.Status404NotFound);
}
