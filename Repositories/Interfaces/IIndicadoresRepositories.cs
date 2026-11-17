using OpMetrics.Core.Entities;

namespace OpMetrics.Core.Repositories.Interfaces;

public interface IQualidadeRepository : IRepository<Qualidade>
{
    Task<IEnumerable<Qualidade>> GetByLinhaAsync(string linha);
    Task<IEnumerable<Qualidade>> GetByDataAsync(DateTime data);
}
