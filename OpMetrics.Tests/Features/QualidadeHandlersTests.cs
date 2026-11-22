using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;
using OpMetrics.Core.Entities;
using OpMetrics.Core.Features.Qualidades;

namespace OpMetrics.Tests.Features;

/// <summary>Testes dos slices de Qualidade, com banco em memória.</summary>
public class QualidadeHandlersTests
{
    private static AppDbContext CriarBanco() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static Qualidade CriarQualidade(string linha, DateTime data) => new()
    {
        Linha = linha,
        Data = data,
        Turno = "Manha",
        TotalInspecionado = 100,
        TotalDefeitos = 5,
        TotalRejeitos = 5
    };

    private static async Task<Qualidade> SalvarAsync(AppDbContext db, Qualidade qualidade)
    {
        db.Qualidades.Add(qualidade);
        await db.SaveChangesAsync();
        return qualidade;
    }

    [Fact]
    public async Task Registrar_SalvaEDevolveIndiceDeQualidade()
    {
        using var db = CriarBanco();
        var request = new RegistrarQualidade.Request("Linha 1", new DateTime(2026, 11, 10), "Manha", 200, 10, 0);

        var resposta = await new RegistrarQualidade.Handler(db).HandleAsync(request, CancellationToken.None);

        var salva = await db.Qualidades.SingleAsync();
        Assert.Equal(resposta.Id, salva.Id);
        Assert.Equal(95, resposta.IndiceQualidade, precision: 2);
    }

    [Fact]
    public async Task ObterPorId_Existente_DevolveORegistro()
    {
        using var db = CriarBanco();
        var qualidade = await SalvarAsync(db, CriarQualidade("Linha 1", new DateTime(2026, 11, 10)));

        var resposta = await new ObterQualidadePorId.Handler(db).HandleAsync(qualidade.Id, CancellationToken.None);

        Assert.NotNull(resposta);
        Assert.Equal(90, resposta.IndiceQualidade, precision: 2);
    }

    [Fact]
    public async Task ObterPorId_Inexistente_DevolveNull()
    {
        using var db = CriarBanco();

        var resposta = await new ObterQualidadePorId.Handler(db).HandleAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(resposta);
    }

    [Fact]
    public async Task Listar_OrdenaDaMaisRecenteParaAMaisAntiga()
    {
        using var db = CriarBanco();
        await SalvarAsync(db, CriarQualidade("Linha 1", new DateTime(2026, 11, 8)));
        await SalvarAsync(db, CriarQualidade("Linha 2", new DateTime(2026, 11, 10)));
        await SalvarAsync(db, CriarQualidade("Linha 1", new DateTime(2026, 11, 9)));

        var lista = await new ListarQualidades.Handler(db).HandleAsync(CancellationToken.None);

        Assert.Equal(
            [new DateTime(2026, 11, 10), new DateTime(2026, 11, 9), new DateTime(2026, 11, 8)],
            lista.Select(x => x.Data));
    }

    [Fact]
    public async Task ListarPorLinha_TrazSoALinhaPedida()
    {
        using var db = CriarBanco();
        await SalvarAsync(db, CriarQualidade("Linha 1", new DateTime(2026, 11, 8)));
        await SalvarAsync(db, CriarQualidade("Linha 2", new DateTime(2026, 11, 9)));
        await SalvarAsync(db, CriarQualidade("Linha 1", new DateTime(2026, 11, 10)));

        var lista = await new ListarQualidadesPorLinha.Handler(db).HandleAsync("Linha 1", CancellationToken.None);

        Assert.Equal(2, lista.Count);
        Assert.All(lista, x => Assert.Equal("Linha 1", x.Linha));
    }

    [Fact]
    public async Task Atualizar_Existente_GravaOsNovosValores()
    {
        using var db = CriarBanco();
        var qualidade = await SalvarAsync(db, CriarQualidade("Linha 1", new DateTime(2026, 11, 10)));
        var request = new AtualizarQualidade.Request("Linha 2", new DateTime(2026, 11, 10), "Tarde", 50, 0, 0);

        var resposta = await new AtualizarQualidade.Handler(db).HandleAsync(qualidade.Id, request, CancellationToken.None);

        Assert.NotNull(resposta);
        Assert.Equal(100, resposta.IndiceQualidade, precision: 2);
        var salva = await db.Qualidades.SingleAsync();
        Assert.Equal("Linha 2", salva.Linha);
        Assert.Equal(50, salva.TotalInspecionado);
        Assert.NotNull(salva.UpdatedAt);
    }

    [Fact]
    public async Task Atualizar_Inexistente_DevolveNull()
    {
        using var db = CriarBanco();
        var request = new AtualizarQualidade.Request("Linha 1", new DateTime(2026, 11, 10), "Manha", 100, 1, 1);

        var resposta = await new AtualizarQualidade.Handler(db).HandleAsync(Guid.NewGuid(), request, CancellationToken.None);

        Assert.Null(resposta);
    }

    [Fact]
    public async Task Remover_Existente_ApagaDoBanco()
    {
        using var db = CriarBanco();
        var qualidade = await SalvarAsync(db, CriarQualidade("Linha 1", new DateTime(2026, 11, 10)));

        var removeu = await new RemoverQualidade.Handler(db).HandleAsync(qualidade.Id, CancellationToken.None);

        Assert.True(removeu);
        Assert.Empty(db.Qualidades);
    }

    [Fact]
    public async Task Remover_Inexistente_DevolveFalse()
    {
        using var db = CriarBanco();

        var removeu = await new RemoverQualidade.Handler(db).HandleAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.False(removeu);
    }
}
