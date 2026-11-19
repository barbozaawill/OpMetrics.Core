using OpMetrics.Core.Entities;

namespace OpMetrics.Core.Features.Producoes;

/// <summary>
/// O que a API devolve sobre uma produção. Compartilhado entre os slices de Produção.
/// O mapeamento é manual e explícito: dá para ver exatamente o que sai na resposta.
/// </summary>
public record ProducaoResposta(
    Guid Id,
    DateTime CreatedAt,
    string Linha,
    DateTime Data,
    string Turno,
    double MetaPecas,
    double PecasProduzidas,
    double PercentualAtingido)
{
    public static ProducaoResposta De(Producao producao) => new(
        producao.Id,
        producao.CreatedAt,
        producao.Linha,
        producao.Data,
        producao.Turno,
        producao.MetaPecas,
        producao.PecasProduzidas,
        producao.PercentualAtingido);
}
