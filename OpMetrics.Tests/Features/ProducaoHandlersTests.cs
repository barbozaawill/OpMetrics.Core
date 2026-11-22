using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;
using OpMetrics.Core.Entities;
using OpMetrics.Core.Features.Producoes;

namespace OpMetrics.Tests.Features;

/// <summary>Testes dos slices de Produção, com banco em memória.</summary>
public class ProducaoHandlersTests
{
    private static AppDbContext CriarBanco() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static Producao CriarProducao(string linha, DateTime data) => new()
    {
        Linha = linha,
        Data = data,
        Turno = "Manha",
        MetaPecas = 1000,
        PecasProduzidas = 750
    };

    private static async Task<Producao> SalvarAsync(AppDbContext db, Producao producao)
    {
        db.Producoes.Add(producao);
        await db.SaveChangesAsync();
        return producao;
    }

    [Fact]
    public async Task Registrar_SalvaEDevolvePercentualAtingido()
    {
        using var db = CriarBanco();
        var request = new RegistrarProducao.Request("Linha 1", new DateTime(2026, 11, 10), "Manha", 1000, 750);

        var resposta = await new RegistrarProducao.Handler(db).HandleAsync(request, CancellationToken.None);

        var salva = await db.Producoes.SingleAsync();
        Assert.Equal(resposta.Id, salva.Id);
        Assert.Equal(75, resposta.PercentualAtingido, precision: 2);
    }

    [Fact]
    public async Task ObterPorId_Existente_DevolveORegistro()
    {
        using var db = CriarBanco();
        var producao = await SalvarAsync(db, CriarProducao("Linha 1", new DateTime(2026, 11, 10)));

        var resposta = await new ObterProducaoPorId.Handler(db).HandleAsync(producao.Id, CancellationToken.None);

        Assert.NotNull(resposta);
        Assert.Equal(producao.Id, resposta.Id);
    }

    [Fact]
    public async Task ObterPorId_Inexistente_DevolveNull()
    {
        using var db = CriarBanco();

        var resposta = await new ObterProducaoPorId.Handler(db).HandleAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(resposta);
    }

    [Fact]
    public async Task Listar_OrdenaDoMaisRecenteParaOMaisAntigo()
    {
        using var db = CriarBanco();
        await SalvarAsync(db, CriarProducao("Linha 1", new DateTime(2026, 11, 8)));
        await SalvarAsync(db, CriarProducao("Linha 2", new DateTime(2026, 11, 10)));
        await SalvarAsync(db, CriarProducao("Linha 1", new DateTime(2026, 11, 9)));

        var lista = await new ListarProducoes.Handler(db).HandleAsync(CancellationToken.None);

        Assert.Equal(
            [new DateTime(2026, 11, 10), new DateTime(2026, 11, 9), new DateTime(2026, 11, 8)],
            lista.Select(x => x.Data));
    }

    [Fact]
    public async Task ListarPorLinha_TrazSoALinhaPedida()
    {
        using var db = CriarBanco();
        await SalvarAsync(db, CriarProducao("Linha 1", new DateTime(2026, 11, 8)));
        await SalvarAsync(db, CriarProducao("Linha 2", new DateTime(2026, 11, 9)));
        await SalvarAsync(db, CriarProducao("Linha 1", new DateTime(2026, 11, 10)));

        var lista = await new ListarProducoesPorLinha.Handler(db).HandleAsync("Linha 1", CancellationToken.None);

        Assert.Equal(2, lista.Count);
        Assert.All(lista, x => Assert.Equal("Linha 1", x.Linha));
    }

    [Fact]
    public async Task Atualizar_Existente_GravaOsNovosValores()
    {
        using var db = CriarBanco();
        var producao = await SalvarAsync(db, CriarProducao("Linha 1", new DateTime(2026, 11, 10)));
        var request = new AtualizarProducao.Request("Linha 2", new DateTime(2026, 11, 10), "Noite", 800, 1000);

        var resposta = await new AtualizarProducao.Handler(db).HandleAsync(producao.Id, request, CancellationToken.None);

        Assert.NotNull(resposta);
        Assert.Equal(125, resposta.PercentualAtingido, precision: 2);
        var salva = await db.Producoes.SingleAsync();
        Assert.Equal("Linha 2", salva.Linha);
        Assert.Equal("Noite", salva.Turno);
        Assert.NotNull(salva.UpdatedAt);
    }

    [Fact]
    public async Task Atualizar_Inexistente_DevolveNull()
    {
        using var db = CriarBanco();
        var request = new AtualizarProducao.Request("Linha 1", new DateTime(2026, 11, 10), "Manha", 1000, 500);

        var resposta = await new AtualizarProducao.Handler(db).HandleAsync(Guid.NewGuid(), request, CancellationToken.None);

        Assert.Null(resposta);
    }

    [Fact]
    public async Task Remover_Existente_ApagaDoBanco()
    {
        using var db = CriarBanco();
        var producao = await SalvarAsync(db, CriarProducao("Linha 1", new DateTime(2026, 11, 10)));

        var removeu = await new RemoverProducao.Handler(db).HandleAsync(producao.Id, CancellationToken.None);

        Assert.True(removeu);
        Assert.Empty(db.Producoes);
    }

    [Fact]
    public async Task Remover_Inexistente_DevolveFalse()
    {
        using var db = CriarBanco();

        var removeu = await new RemoverProducao.Handler(db).HandleAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.False(removeu);
    }
}
