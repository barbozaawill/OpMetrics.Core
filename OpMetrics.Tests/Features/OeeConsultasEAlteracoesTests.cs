using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;
using OpMetrics.Core.Entities;
using OpMetrics.Core.Features.Oees;

namespace OpMetrics.Tests.Features;

/// <summary>Testes dos slices de consulta, atualização e remoção de OEE.</summary>
public class OeeConsultasEAlteracoesTests
{
    private static AppDbContext CriarBanco() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static Oee CriarOee(string linha, DateTime data) => new()
    {
        Linha = linha,
        Data = data,
        Turno = "Manha",
        TempoPlaneadoMinutos = 480,
        TempoRodandoMinutos = 432,
        CapacidadeIdealPecas = 1000,
        PecasProduzidas = 950,
        PecasBoas = 931
    };

    private static async Task<Oee> SalvarAsync(AppDbContext db, Oee oee)
    {
        db.Oees.Add(oee);
        await db.SaveChangesAsync();
        return oee;
    }

    // ---- ObterOeePorId ----

    [Fact]
    public async Task ObterPorId_Existente_DevolveORegistro()
    {
        using var db = CriarBanco();
        var oee = await SalvarAsync(db, CriarOee("Linha 1", new DateTime(2026, 11, 10)));

        var resposta = await new ObterOeePorId.Handler(db).HandleAsync(oee.Id, CancellationToken.None);

        Assert.NotNull(resposta);
        Assert.Equal(oee.Id, resposta.Id);
        Assert.Equal(83.79, resposta.OeePercentual, precision: 2);
    }

    [Fact]
    public async Task ObterPorId_Inexistente_DevolveNull()
    {
        using var db = CriarBanco();

        var resposta = await new ObterOeePorId.Handler(db).HandleAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(resposta);
    }

    // ---- ListarOees / ListarOeesPorLinha ----

    [Fact]
    public async Task Listar_OrdenaDoMaisRecenteParaOMaisAntigo()
    {
        using var db = CriarBanco();
        await SalvarAsync(db, CriarOee("Linha 1", new DateTime(2026, 11, 8)));
        await SalvarAsync(db, CriarOee("Linha 2", new DateTime(2026, 11, 10)));
        await SalvarAsync(db, CriarOee("Linha 1", new DateTime(2026, 11, 9)));

        var lista = await new ListarOees.Handler(db).HandleAsync(CancellationToken.None);

        Assert.Equal(
            [new DateTime(2026, 11, 10), new DateTime(2026, 11, 9), new DateTime(2026, 11, 8)],
            lista.Select(x => x.Data));
    }

    [Fact]
    public async Task ListarPorLinha_TrazSoALinhaPedida()
    {
        using var db = CriarBanco();
        await SalvarAsync(db, CriarOee("Linha 1", new DateTime(2026, 11, 8)));
        await SalvarAsync(db, CriarOee("Linha 2", new DateTime(2026, 11, 9)));
        await SalvarAsync(db, CriarOee("Linha 1", new DateTime(2026, 11, 10)));

        var lista = await new ListarOeesPorLinha.Handler(db).HandleAsync("Linha 1", CancellationToken.None);

        Assert.Equal(2, lista.Count);
        Assert.All(lista, x => Assert.Equal("Linha 1", x.Linha));
    }

    // ---- AtualizarOee ----

    [Fact]
    public async Task Atualizar_Existente_GravaOsNovosValores()
    {
        using var db = CriarBanco();
        var oee = await SalvarAsync(db, CriarOee("Linha 1", new DateTime(2026, 11, 10)));
        var request = new AtualizarOee.Request(
            Linha: "Linha 3",
            Data: new DateTime(2026, 11, 10),
            Turno: "Tarde",
            TempoPlaneadoMinutos: 480,
            TempoRodandoMinutos: 480,
            CapacidadeIdealPecas: 1000,
            PecasProduzidas: 1000,
            PecasBoas: 1000);

        var resposta = await new AtualizarOee.Handler(db).HandleAsync(oee.Id, request, CancellationToken.None);

        Assert.NotNull(resposta);
        Assert.Equal(100, resposta.OeePercentual, precision: 2);
        var salvo = await db.Oees.SingleAsync();
        Assert.Equal("Linha 3", salvo.Linha);
        Assert.Equal("Tarde", salvo.Turno);
        Assert.NotNull(salvo.UpdatedAt);
    }

    [Fact]
    public async Task Atualizar_Inexistente_DevolveNull()
    {
        using var db = CriarBanco();
        var request = new AtualizarOee.Request("Linha 1", new DateTime(2026, 11, 10), "Manha", 480, 400, 1000, 900, 850);

        var resposta = await new AtualizarOee.Handler(db).HandleAsync(Guid.NewGuid(), request, CancellationToken.None);

        Assert.Null(resposta);
    }

    // ---- RemoverOee ----

    [Fact]
    public async Task Remover_Existente_ApagaDoBanco()
    {
        using var db = CriarBanco();
        var oee = await SalvarAsync(db, CriarOee("Linha 1", new DateTime(2026, 11, 10)));

        var removeu = await new RemoverOee.Handler(db).HandleAsync(oee.Id, CancellationToken.None);

        Assert.True(removeu);
        Assert.Empty(db.Oees);
    }

    [Fact]
    public async Task Remover_Inexistente_DevolveFalse()
    {
        using var db = CriarBanco();

        var removeu = await new RemoverOee.Handler(db).HandleAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.False(removeu);
    }
}
