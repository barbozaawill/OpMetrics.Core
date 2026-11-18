using OpMetrics.Core.Features.Oees;
using OpMetrics.Core.Features.Producoes;
using OpMetrics.Core.Features.Qualidades;

namespace OpMetrics.Tests.Validators;

public class UpdateValidatorsTests
{
    [Fact]
    public void AtualizarOee_Valido_Passa()
    {
        var request = new AtualizarOee.Request(
            Linha: "Linha 1",
            Data: DateTime.UtcNow.AddDays(-1),
            Turno: "Noite",
            TempoPlaneadoMinutos: 480,
            TempoRodandoMinutos: 480,
            CapacidadeIdealPecas: 1000,
            PecasProduzidas: 1000,
            PecasBoas: 1000);

        Assert.True(new AtualizarOee.Validator().Validate(request).IsValid);
    }

    [Fact]
    public void AtualizarOee_PecasBoasMaiorQueProduzidas_EhInvalido()
    {
        var request = new AtualizarOee.Request(
            Linha: "Linha 1",
            Data: DateTime.UtcNow.AddDays(-1),
            Turno: "Manha",
            TempoPlaneadoMinutos: 480,
            TempoRodandoMinutos: 400,
            CapacidadeIdealPecas: 1000,
            PecasProduzidas: 900,
            PecasBoas: 950);

        var resultado = new AtualizarOee.Validator().Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(AtualizarOee.Request.PecasBoas));
    }

    [Fact]
    public void AtualizarProducao_MetaZero_EhInvalido()
    {
        var request = new AtualizarProducao.Request(
            Linha: "Linha 1",
            Data: DateTime.UtcNow.AddDays(-1),
            Turno: "Tarde",
            MetaPecas: 0,
            PecasProduzidas: 100);

        var resultado = new AtualizarProducao.Validator().Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(AtualizarProducao.Request.MetaPecas));
    }

    [Fact]
    public void AtualizarProducao_Valido_Passa()
    {
        var request = new AtualizarProducao.Request(
            Linha: "Linha 1",
            Data: DateTime.UtcNow.AddDays(-1),
            Turno: "Tarde",
            MetaPecas: 1000,
            PecasProduzidas: 1100);

        Assert.True(new AtualizarProducao.Validator().Validate(request).IsValid);
    }

    [Fact]
    public void AtualizarQualidade_DefeitosMaisRejeitosAcimaDoInspecionado_EhInvalido()
    {
        var request = new AtualizarQualidade.Request(
            Linha: "Linha 1",
            Data: DateTime.UtcNow.AddDays(-1),
            Turno: "Manha",
            TotalInspecionado: 100,
            TotalDefeitos: 70,
            TotalRejeitos: 40);

        Assert.False(new AtualizarQualidade.Validator().Validate(request).IsValid);
    }

    [Fact]
    public void AtualizarQualidade_TurnoInvalido_EhRejeitado()
    {
        var request = new AtualizarQualidade.Request(
            Linha: "Linha 1",
            Data: DateTime.UtcNow.AddDays(-1),
            Turno: "Madrugada",
            TotalInspecionado: 100,
            TotalDefeitos: 0,
            TotalRejeitos: 0);

        var resultado = new AtualizarQualidade.Validator().Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(AtualizarQualidade.Request.Turno));
    }
}
