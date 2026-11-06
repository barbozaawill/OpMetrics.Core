using OpMetrics.Core.DTOs.Requests;
using OpMetrics.Core.Validators;

namespace OpMetrics.Tests.Validators;

public class QualidadeValidatorTests
{
    private readonly CreateQualidadeValidator _validator = new();

    private static CreateQualidadeRequest CriarRequest(int inspecionado, int defeitos, int rejeitos) => new()
    {
        Linha = "Linha 1",
        Data = DateTime.UtcNow.AddDays(-1),
        Turno = "Tarde",
        TotalInspecionado = inspecionado,
        TotalDefeitos = defeitos,
        TotalRejeitos = rejeitos
    };

    [Theory]
    [InlineData(100, 0, 0)]
    [InlineData(100, 10, 5)]
    [InlineData(100, 60, 40)] // soma exatamente igual ao inspecionado
    public void SomaDentroDoInspecionado_EhValido(int inspecionado, int defeitos, int rejeitos)
    {
        Assert.True(_validator.Validate(CriarRequest(inspecionado, defeitos, rejeitos)).IsValid);
    }

    [Fact]
    public void DefeitosMaisRejeitosAcimaDoInspecionado_EhInvalido()
    {
        // 60 + 60 > 100: antes passava (cada um < 100) e gerava IndiceQualidade negativo.
        var resultado = _validator.Validate(CriarRequest(100, 60, 60));

        Assert.False(resultado.IsValid);
    }

    [Fact]
    public void ValoresNegativos_SaoRejeitados()
    {
        var resultado = _validator.Validate(CriarRequest(100, -1, -1));

        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(CreateQualidadeRequest.TotalDefeitos));
        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(CreateQualidadeRequest.TotalRejeitos));
    }
}
