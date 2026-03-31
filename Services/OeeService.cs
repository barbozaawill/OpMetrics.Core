using AutoMapper;
using OpMetrics.Core.DTOs.Requests;
using OpMetrics.Core.DTOs.Responses;
using OpMetrics.Core.Entities;
using OpMetrics.Core.Repositories.Interfaces;

namespace OpMetrics.Core.Services;

public class OeeService : IOeeService
{
    private readonly IOeeRepository _repository;
    private readonly IMapper _mapper;

    public OeeService(IOeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<OeeResponse> CreateAsync(CreateOeeRequest request)
    {
        var entity = _mapper.Map<Oee>(request);
        await _repository.CreateAsync(entity);
        return _mapper.Map<OeeResponse>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);

    }

    public async Task<IEnumerable<OeeResponse>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<OeeResponse>>(entities);
    }

    public async Task<OeeResponse?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null) return null;

        return _mapper.Map<OeeResponse>(entity);
    }

    public async Task<IEnumerable<OeeResponse>> GetByLinhaAsync(string linha)
    {
        var entities = await _repository.GetByLinhaAsync(linha);
        return _mapper.Map<IEnumerable<OeeResponse>>(entities);
    }

    public async Task<OeeResponse?> UpdateAsync(Guid id, UpdateOeeRequest request)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return null;
        _mapper.Map(request, entity);
        await _repository.UpdateAsync(entity);
        return _mapper.Map<OeeResponse>(entity);

    }
}
