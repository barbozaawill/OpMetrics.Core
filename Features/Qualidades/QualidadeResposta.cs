using OpMetrics.Core.Entities;

namespace OpMetrics.Core.Features.Qualidades;

/// <summary>
/// O que a API devolve sobre uma inspeção de qualidade. Compartilhado entre os slices de Qualidade.
/// O mapeamento é manual e explícito: dá para ver exatamente o que sai na resposta.
/// </summary>
public record QualidadeResposta(
    Guid Id,
    DateTime CreatedAt,
    string Linha,
    DateTime Data,
    string Turno,
    int TotalInspecionado,
    int TotalDefeitos,
    int TotalRejeitos,
    double IndiceQualidade)
{
    public static QualidadeResposta De(Qualidade qualidade) => new(
        qualidade.Id,
        qualidade.CreatedAt,
        qualidade.Linha,
        qualidade.Data,
        qualidade.Turno,
        qualidade.TotalInspecionado,
        qualidade.TotalDefeitos,
        qualidade.TotalRejeitos,
        qualidade.IndiceQualidade);
}
