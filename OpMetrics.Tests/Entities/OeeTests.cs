using OpMetrics.Core.Entities;

namespace OpMetrics.Tests.Entities;

public class OeeTests
{
    private static Oee CriarOeeExemplo() => new()
    {
        TempoPlaneadoMinutos = 480,
        TempoRodandoMinutos = 432,
        CapacidadeIdealPecas = 1000,
        PecasProduzidas = 950,
        PecasBoas = 931
    };

    [Fact]
    public void Disponibilidade_EhTempoRodandoSobreTempoPlaneado()
    {
        var oee = CriarOeeExemplo();

        Assert.Equal(90, oee.Disponibilidade, precision: 2);
    }

    [Fact]
    public void Performance_EhPecasProduzidasSobreCapacidadeIdeal()
    {
        var oee = CriarOeeExemplo();

        Assert.Equal(95, oee.Performance, precision: 2);
    }

    [Fact]
    public void IndiceQualidade_EhPecasBoasSobrePecasProduzidas()
    {
        var oee = CriarOeeExemplo();

        Assert.Equal(98, oee.IndiceQualidade, precision: 2);
    }

    [Fact]
    public void OeePercentual_EhProdutoDosTresIndicadores()
    {
        var oee = CriarOeeExemplo();

        Assert.Equal(83.79, oee.OeePercentual, precision: 2);
    }

    [Fact]
    public void SemPecasProduzidas_IndiceQualidadeEhZero_SemDividirPorZero()
    {
        var oee = CriarOeeExemplo();
        oee.PecasProduzidas = 0;
        oee.PecasBoas = 0;

        Assert.Equal(0, oee.IndiceQualidade);
        Assert.Equal(0, oee.OeePercentual);
    }

    [Fact]
    public void SemTempoPlaneado_DisponibilidadeEhZero()
    {
        var oee = CriarOeeExemplo();
        oee.TempoPlaneadoMinutos = 0;

        Assert.Equal(0, oee.Disponibilidade);
    }

    [Fact]
    public void TurnoPerfeito_TemOeeDe100()
    {
        var oee = new Oee
        {
            TempoPlaneadoMinutos = 480,
            TempoRodandoMinutos = 480,
            CapacidadeIdealPecas = 1000,
            PecasProduzidas = 1000,
            PecasBoas = 1000
        };

        Assert.Equal(100, oee.OeePercentual, precision: 2);
    }
}
