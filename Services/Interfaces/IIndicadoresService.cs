using OpMetrics.Core.DTOs.Requests;
using OpMetrics.Core.DTOs.Responses;

namespace OpMetrics.Core.Services;

public interface IQualidadeService
{
    Task<IEnumerable<QualidadeResponse>> GetAllAsync();
    Task<QualidadeResponse?> GetByIdAsync(Guid id);
    Task<IEnumerable<QualidadeResponse>> GetByLinhaAsync(string linha);
    Task<QualidadeResponse> CreateAsync(CreateQualidadeRequest request);
    Task<QualidadeResponse?> UpdateAsync(Guid id, UpdateQualidadeRequest request);
    Task<bool> DeleteAsync(Guid id);
}
