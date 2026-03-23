using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;
using OpMetrics.Core.Entities;
using OpMetrics.Core.Repositories.Interfaces;

namespace OpMetrics.Core.Repositories;

public class ProducaoRepository : IProducaoRepository
{
    private readonly AppDbContext _context;

    public ProducaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Producao> CreateAsync(Producao entity)
    {
        _context.Producoes.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Producoes.FindAsync(id);

        if (entity is null) return false;

        _context.Producoes.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
        
    }

    public async Task<IEnumerable<Producao>> GetAllAsync()
    {
        return await _context.Producoes
            .OrderByDescending(x => x.Data)
            .ToListAsync();
    }

    public async Task<IEnumerable<Producao>> GetByDataAsync(DateTime data)
    {
        return await _context.Producoes
            .Where(x => x.Data.Date == data.Date)
            .OrderByDescending(x => x.Data)
            .ToListAsync();
    }

    public async Task<Producao?> GetByIdAsync(Guid id)
    {
        return await _context.Producoes.FindAsync(id);
    }

    public async Task<IEnumerable<Producao>> GetByLinhaAsync(string linha)
    {
        return await _context.Producoes
            .Where(x => x.Linha == linha)
            .OrderByDescending (x => x.Data)
            .ToListAsync();
    }

    public async Task<Producao> UpdateAsync(Producao entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Producoes.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
