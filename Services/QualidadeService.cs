using AutoMapper;
using OpMetrics.Core.DTOs.Requests;
using OpMetrics.Core.DTOs.Responses;
using OpMetrics.Core.Entities;
using OpMetrics.Core.Repositories.Interfaces;

namespace OpMetrics.Core.Services;

public class QualidadeService : IQualidadeService
{
    private readonly IQualidadeRepository _repository;
    private readonly IMapper _mapper;

    public QualidadeService(IQualidadeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<QualidadeResponse> CreateAsync(CreateQualidadeRequest request)
    {
        var entity = _mapper.Map<Qualidade>(request);
        await _repository.CreateAsync(entity);
        return _mapper.Map<QualidadeResponse>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);

    }

    public async Task<IEnumerable<QualidadeResponse>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<QualidadeResponse>>(entities);
    }

    public async Task<QualidadeResponse?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null) return null;

        return _mapper.Map<QualidadeResponse>(entity);
    }

    public async Task<IEnumerable<QualidadeResponse>> GetByLinhaAsync(string linha)
    {
        var entities = await _repository.GetByLinhaAsync(linha);
        return _mapper.Map<IEnumerable<QualidadeResponse>>(entities);
    }

    public async Task<QualidadeResponse?> UpdateAsync(Guid id, UpdateQualidadeRequest request)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return null;
        _mapper.Map(request, entity);
        await _repository.UpdateAsync(entity);
        return _mapper.Map<QualidadeResponse>(entity);

    }
}
