using OpMetrics.Core.Entities;

namespace OpMetrics.Core.Repositories.Interfaces;

public interface IProducaoRepository : IRepository<Producao>
{
    Task<IEnumerable<Producao>> GetByLinhaAsync(string linha);
    Task<IEnumerable<Producao>> GetByDataAsync(DateTime data);
}

public interface IQualidadeRepository : IRepository<Qualidade>
{
    Task<IEnumerable<Qualidade>> GetByLinhaAsync(string linha);
    Task<IEnumerable<Qualidade>> GetByDataAsync(DateTime data);
}
