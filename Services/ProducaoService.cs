using AutoMapper;
using OpMetrics.Core.DTOs.Requests;
using OpMetrics.Core.DTOs.Responses;
using OpMetrics.Core.Entities;
using OpMetrics.Core.Repositories.Interfaces;

namespace OpMetrics.Core.Services;

public class ProducaoService : IProducaoService
{
    private readonly IProducaoRepository _repository;
    private readonly IMapper _mapper;

    public ProducaoService(IProducaoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProducaoResponse> CreateAsync(CreateProducaoRequest request)
    {
        var entity = _mapper.Map<Producao>(request);
        await _repository.CreateAsync(entity);
        return _mapper.Map<ProducaoResponse>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);

    }

    public async Task<IEnumerable<ProducaoResponse>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<ProducaoResponse>>(entities);
    }

    public async Task<ProducaoResponse?> GetByIdAsync(Guid id)
    {
       var entity = await _repository.GetByIdAsync(id);

        if (entity == null) return null;
  
            return _mapper.Map<ProducaoResponse>(entity);
    }

    public async Task<IEnumerable<ProducaoResponse>> GetByLinhaAsync(string linha)
    {
        var entities = await _repository.GetByLinhaAsync(linha);
        return _mapper.Map<IEnumerable<ProducaoResponse>>(entities);
    }

    public async Task<ProducaoResponse?> UpdateAsync(Guid id, UpdateProducaoRequest request)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return null;
        _mapper.Map(request, entity);
        await _repository.UpdateAsync(entity);
        return _mapper.Map<ProducaoResponse>(entity);

    }
}
