using OpMetrics.Core.Entities;

namespace OpMetrics.Core.Features.Oees;

/// <summary>
/// O que a API devolve sobre um OEE. Compartilhado entre os slices de OEE,
/// porque todos devolvem o mesmo formato.
/// O mapeamento é manual e explícito: dá para ver exatamente o que sai na resposta.
/// </summary>
public record OeeResposta(
    Guid Id,
    DateTime CreatedAt,
    string Linha,
    DateTime Data,
    string Turno,
    double TempoPlaneadoMinutos,
    double TempoRodandoMinutos,
    double Disponibilidade,
    double CapacidadeIdealPecas,
    double PecasProduzidas,
    double Performance,
    double PecasBoas,
    double IndiceQualidade,
    double OeePercentual)
{
    public static OeeResposta De(Oee oee) => new(
        oee.Id,
        oee.CreatedAt,
        oee.Linha,
        oee.Data,
        oee.Turno,
        oee.TempoPlaneadoMinutos,
        oee.TempoRodandoMinutos,
        oee.Disponibilidade,
        oee.CapacidadeIdealPecas,
        oee.PecasProduzidas,
        oee.Performance,
        oee.PecasBoas,
        oee.IndiceQualidade,
        oee.OeePercentual);
}
