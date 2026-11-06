using OpMetrics.Core.DTOs.Requests;
using OpMetrics.Core.Validators;

namespace OpMetrics.Tests.Validators;

// O Update usa as mesmas regras do Create (via Include), entao aqui
// o foco e garantir que o PUT nao aceita mais dados invalidos.
public class UpdateValidatorsTests
{
    [Fact]
    public void UpdateOee_Valido_Passa()
    {
        var request = new UpdateOeeRequest
        {
            Linha = "Linha 1",
            Data = DateTime.UtcNow.AddDays(-1),
            Turno = "Noite",
            TempoPlaneadoMinutos = 480,
            TempoRodandoMinutos = 480,
            CapacidadeIdealPecas = 1000,
            PecasProduzidas = 1000,
            PecasBoas = 1000
        };

        Assert.True(new UpdateOeeValidator().Validate(request).IsValid);
    }

    [Fact]
    public void UpdateOee_PecasBoasMaiorQueProduzidas_EhInvalido()
    {
        var request = new UpdateOeeRequest
        {
            Linha = "Linha 1",
            Data = DateTime.UtcNow.AddDays(-1),
            Turno = "Manha",
            TempoPlaneadoMinutos = 480,
            TempoRodandoMinutos = 400,
            CapacidadeIdealPecas = 1000,
            PecasProduzidas = 900,
            PecasBoas = 950
        };

        var resultado = new UpdateOeeValidator().Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(UpdateOeeRequest.PecasBoas));
    }

    [Fact]
    public void UpdateProducao_MetaZero_EhInvalido()
    {
        var request = new UpdateProducaoRequest
        {
            Linha = "Linha 1",
            Data = DateTime.UtcNow.AddDays(-1),
            Turno = "Tarde",
            MetaPecas = 0,
            PecasProduzidas = 100
        };

        var resultado = new UpdateProducaoValidator().Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(UpdateProducaoRequest.MetaPecas));
    }

    [Fact]
    public void UpdateProducao_Valido_Passa()
    {
        var request = new UpdateProducaoRequest
        {
            Linha = "Linha 1",
            Data = DateTime.UtcNow.AddDays(-1),
            Turno = "Tarde",
            MetaPecas = 1000,
            PecasProduzidas = 1100
        };

        Assert.True(new UpdateProducaoValidator().Validate(request).IsValid);
    }

    [Fact]
    public void UpdateQualidade_DefeitosMaisRejeitosAcimaDoInspecionado_EhInvalido()
    {
        var request = new UpdateQualidadeRequest
        {
            Linha = "Linha 1",
            Data = DateTime.UtcNow.AddDays(-1),
            Turno = "Manha",
            TotalInspecionado = 100,
            TotalDefeitos = 70,
            TotalRejeitos = 40
        };

        Assert.False(new UpdateQualidadeValidator().Validate(request).IsValid);
    }

    [Fact]
    public void UpdateQualidade_TurnoInvalido_EhRejeitado()
    {
        var request = new UpdateQualidadeRequest
        {
            Linha = "Linha 1",
            Data = DateTime.UtcNow.AddDays(-1),
            Turno = "Madrugada",
            TotalInspecionado = 100
        };

        var resultado = new UpdateQualidadeValidator().Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(UpdateQualidadeRequest.Turno));
    }
}
