using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;
using OpMetrics.Core.Features.Oees;

namespace OpMetrics.Tests.Features;

public class RegistrarOeeTests
{
    // Banco em memória: cada teste ganha um banco novo (nome aleatório),
    // então um teste não interfere no outro.
    private static AppDbContext CriarBanco() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static RegistrarOee.Request CriarRequest() => new(
        Linha: "Linha 1",
        Data: new DateTime(2026, 11, 10, 8, 0, 0, DateTimeKind.Utc),
        Turno: "Manha",
        TempoPlaneadoMinutos: 480,
        TempoRodandoMinutos: 432,
        CapacidadeIdealPecas: 1000,
        PecasProduzidas: 950,
        PecasBoas: 931);

    [Fact]
    public async Task Registrar_SalvaNoBanco()
    {
        using var db = CriarBanco();
        var handler = new RegistrarOee.Handler(db);

        var resposta = await handler.HandleAsync(CriarRequest(), CancellationToken.None);

        var salvo = await db.Oees.SingleAsync();
        Assert.Equal(resposta.Id, salvo.Id);
        Assert.Equal("Linha 1", salvo.Linha);
        Assert.Equal(931, salvo.PecasBoas);
    }

    [Fact]
    public async Task Registrar_DevolveMetricasCalculadas()
    {
        using var db = CriarBanco();
        var handler = new RegistrarOee.Handler(db);

        var resposta = await handler.HandleAsync(CriarRequest(), CancellationToken.None);

        Assert.Equal(90, resposta.Disponibilidade, precision: 2);
        Assert.Equal(95, resposta.Performance, precision: 2);
        Assert.Equal(98, resposta.IndiceQualidade, precision: 2);
        Assert.Equal(83.79, resposta.OeePercentual, precision: 2);
    }
}
