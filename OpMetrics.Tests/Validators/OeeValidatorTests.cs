using OpMetrics.Core.DTOs.Requests;
using OpMetrics.Core.Validators;

namespace OpMetrics.Tests.Validators;

public class OeeValidatorTests
{
    private readonly CreateOeeValidator _validator = new();

    private static CreateOeeRequest CriarRequestValido() => new()
    {
        Linha = "Linha 1",
        Data = DateTime.UtcNow.AddDays(-1),
        Turno = "Manha",
        TempoPlaneadoMinutos = 480,
        TempoRodandoMinutos = 432,
        CapacidadeIdealPecas = 1000,
        PecasProduzidas = 950,
        PecasBoas = 931
    };

    [Fact]
    public void RequestValido_PassaNaValidacao()
    {
        var resultado = _validator.Validate(CriarRequestValido());

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void CemPorCentoDePecasBoas_EhValido()
    {
        // Antes da correcao, LessThan rejeitava um turno sem nenhuma peca ruim.
        var request = CriarRequestValido();
        request.PecasBoas = request.PecasProduzidas;

        Assert.True(_validator.Validate(request).IsValid);
    }

    [Fact]
    public void TurnoSemParada_EhValido()
    {
        var request = CriarRequestValido();
        request.TempoRodandoMinutos = request.TempoPlaneadoMinutos;

        Assert.True(_validator.Validate(request).IsValid);
    }

    [Fact]
    public void PecasBoasMaiorQueProduzidas_EhInvalido()
    {
        var request = CriarRequestValido();
        request.PecasBoas = request.PecasProduzidas + 1;

        var resultado = _validator.Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(CreateOeeRequest.PecasBoas));
    }

    [Fact]
    public void TempoRodandoMaiorQuePlaneado_EhInvalido()
    {
        var request = CriarRequestValido();
        request.TempoRodandoMinutos = request.TempoPlaneadoMinutos + 1;

        var resultado = _validator.Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(CreateOeeRequest.TempoRodandoMinutos));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Madrugada")]
    public void TurnoInvalido_EhRejeitado(string turno)
    {
        var request = CriarRequestValido();
        request.Turno = turno;

        var resultado = _validator.Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(CreateOeeRequest.Turno));
    }

    [Fact]
    public void DataFutura_EhRejeitada()
    {
        var request = CriarRequestValido();
        request.Data = DateTime.UtcNow.AddDays(1);

        var resultado = _validator.Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(CreateOeeRequest.Data));
    }
}
