using OpMetrics.Core.Entities;

namespace OpMetrics.Tests.Entities;

public class ProducaoTests
{
    [Theory]
    [InlineData(1000, 1000, 100)]
    [InlineData(1000, 750, 75)]
    [InlineData(1000, 1200, 120)]
    [InlineData(0, 500, 0)]
    public void PercentualAtingido_EhPecasProduzidasSobreMeta(double meta, double produzidas, double esperado)
    {
        var producao = new Producao { MetaPecas = meta, PecasProduzidas = produzidas };

        Assert.Equal(esperado, producao.PercentualAtingido, precision: 2);
    }
}

public class QualidadeTests
{
    [Theory]
    [InlineData(100, 0, 0, 100)]
    [InlineData(100, 5, 5, 90)]
    [InlineData(200, 10, 0, 95)]
    [InlineData(0, 0, 0, 0)]
    public void IndiceQualidade_DescontaDefeitosERejeitos(int inspecionado, int defeitos, int rejeitos, double esperado)
    {
        var qualidade = new Qualidade
        {
            TotalInspecionado = inspecionado,
            TotalDefeitos = defeitos,
            TotalRejeitos = rejeitos
        };

        Assert.Equal(esperado, qualidade.IndiceQualidade, precision: 2);
    }

    [Fact]
    public void IndiceQualidade_NaoSofreDivisaoInteira()
    {
        var qualidade = new Qualidade { TotalInspecionado = 3, TotalDefeitos = 1 };

        Assert.Equal(66.67, qualidade.IndiceQualidade, precision: 2);
    }
}
