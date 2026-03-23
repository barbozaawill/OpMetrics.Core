using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;
using OpMetrics.Core.Entities;
using OpMetrics.Core.Repositories.Interfaces;

namespace OpMetrics.Core.Repositories;

public class OeeRepository : IOeeRepository
{
    private readonly AppDbContext _context;

    public OeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Oee> CreateAsync(Oee entity)
    {
        _context.Oees.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Oees.FindAsync(id);

        if (entity is null) return false;

        _context.Oees.Remove(entity);
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<IEnumerable<Oee>> GetAllAsync()
    {
        return await _context.Oees
            .OrderByDescending(x => x.Data)
            .ToListAsync();
    }

    public async Task<IEnumerable<Oee>> GetByDataAsync(DateTime data)
    {
        return await _context.Oees
            .Where(x => x.Data.Date == data.Date)
            .OrderByDescending(x => x.Data)
            .ToListAsync();
    }

    public async Task<Oee?> GetByIdAsync(Guid id)
    {
        return await _context.Oees.FindAsync(id);
    }

    public async Task<IEnumerable<Oee>> GetByLinhaAsync(string linha)
    {
        return await _context.Oees
            .Where(x => x.Linha == linha)
            .OrderByDescending(x => x.Data)
            .ToListAsync();
    }

    public async Task<Oee> UpdateAsync(Oee entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Oees.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
