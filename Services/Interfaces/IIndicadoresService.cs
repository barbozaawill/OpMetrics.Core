using OpMetrics.Core.DTOs.Requests;
using OpMetrics.Core.DTOs.Responses;

namespace OpMetrics.Core.Services;

public interface IProducaoService
{
    Task<IEnumerable<ProducaoResponse>> GetAllAsync();
    Task<ProducaoResponse?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProducaoResponse>> GetByLinhaAsync(string linha);
    Task<ProducaoResponse> CreateAsync(CreateProducaoRequest request);
    Task<ProducaoResponse?> UpdateAsync(Guid id, UpdateProducaoRequest request);
    Task<bool> DeleteAsync(Guid id);
}

public interface IQualidadeService
{
    Task<IEnumerable<QualidadeResponse>> GetAllAsync();
    Task<QualidadeResponse?> GetByIdAsync(Guid id);
    Task<IEnumerable<QualidadeResponse>> GetByLinhaAsync(string linha);
    Task<QualidadeResponse> CreateAsync(CreateQualidadeRequest request);
    Task<QualidadeResponse?> UpdateAsync(Guid id, UpdateQualidadeRequest request);
    Task<bool> DeleteAsync(Guid id);
}
