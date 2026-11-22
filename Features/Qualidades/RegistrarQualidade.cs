using FluentValidation;
using OpMetrics.Core.Common;
using OpMetrics.Core.Data;
using OpMetrics.Core.Entities;

namespace OpMetrics.Core.Features.Qualidades;

/// <summary>Slice "Registrar inspeção de qualidade": request, validação, regra e rota num lugar só.</summary>
public static class RegistrarQualidade
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
        public async Task<QualidadeResposta> HandleAsync(Request request, CancellationToken ct)
        {
            var qualidade = new Qualidade
            {
                Linha = request.Linha,
                Data = request.Data,
                Turno = request.Turno,
                TotalInspecionado = request.TotalInspecionado,
                TotalDefeitos = request.TotalDefeitos,
                TotalRejeitos = request.TotalRejeitos
            };

            db.Qualidades.Add(qualidade);
            await db.SaveChangesAsync(ct);

            return QualidadeResposta.De(qualidade);
        }
    }

    public static void Map(RouteGroupBuilder grupo) =>
        grupo.MapPost("/", async (Request request, Handler handler, CancellationToken ct) =>
            {
                var resposta = await handler.HandleAsync(request, ct);
                return Results.CreatedAtRoute("ObterQualidadePorId", new { id = resposta.Id }, resposta);
            })
            .ComValidacao<Request>()
            .WithName("RegistrarQualidade")
            .Produces<QualidadeResposta>(StatusCodes.Status201Created);
}
