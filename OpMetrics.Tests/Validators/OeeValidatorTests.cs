using OpMetrics.Core.Features.Oees;

namespace OpMetrics.Tests.Validators;

public class OeeValidatorTests
{
    private readonly RegistrarOee.Validator _validator = new();

    private static RegistrarOee.Request CriarRequestValido() => new(
        Linha: "Linha 1",
        Data: DateTime.UtcNow.AddDays(-1),
        Turno: "Manha",
        TempoPlaneadoMinutos: 480,
        TempoRodandoMinutos: 432,
        CapacidadeIdealPecas: 1000,
        PecasProduzidas: 950,
        PecasBoas: 931);

    [Fact]
    public void RequestValido_PassaNaValidacao()
    {
        var resultado = _validator.Validate(CriarRequestValido());

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void CemPorCentoDePecasBoas_EhValido()
    {
        var valido = CriarRequestValido();
        var request = valido with { PecasBoas = valido.PecasProduzidas };

        Assert.True(_validator.Validate(request).IsValid);
    }

    [Fact]
    public void TurnoSemParada_EhValido()
    {
        var valido = CriarRequestValido();
        var request = valido with { TempoRodandoMinutos = valido.TempoPlaneadoMinutos };

        Assert.True(_validator.Validate(request).IsValid);
    }

    [Fact]
    public void PecasBoasMaiorQueProduzidas_EhInvalido()
    {
        var valido = CriarRequestValido();
        var request = valido with { PecasBoas = valido.PecasProduzidas + 1 };

        var resultado = _validator.Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(RegistrarOee.Request.PecasBoas));
    }

    [Fact]
    public void TempoRodandoMaiorQuePlaneado_EhInvalido()
    {
        var valido = CriarRequestValido();
        var request = valido with { TempoRodandoMinutos = valido.TempoPlaneadoMinutos + 1 };

        var resultado = _validator.Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(RegistrarOee.Request.TempoRodandoMinutos));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Madrugada")]
    public void TurnoInvalido_EhRejeitado(string turno)
    {
        var request = CriarRequestValido() with { Turno = turno };

        var resultado = _validator.Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(RegistrarOee.Request.Turno));
    }

    [Fact]
    public void DataFutura_EhRejeitada()
    {
        var request = CriarRequestValido() with { Data = DateTime.UtcNow.AddDays(1) };

        var resultado = _validator.Validate(request);

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(RegistrarOee.Request.Data));
    }
}
