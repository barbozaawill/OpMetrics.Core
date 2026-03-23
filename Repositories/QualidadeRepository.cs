using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Data;
using OpMetrics.Core.Entities;
using OpMetrics.Core.Repositories.Interfaces;

namespace OpMetrics.Core.Repositories;

public class QualidadeRepository : IQualidadeRepository
{
    private readonly AppDbContext _context;

    public QualidadeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Qualidade> CreateAsync(Qualidade entity)
    {
        _context.Qualidades.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Qualidades.FindAsync(id);

        if (entity is null) return false;

        _context.Qualidades.Remove(entity);
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<IEnumerable<Qualidade>> GetAllAsync()
    {
        return await _context.Qualidades
            .OrderByDescending(x => x.Data)
            .ToListAsync();
    }

    public async Task<IEnumerable<Qualidade>> GetByDataAsync(DateTime data)
    {
        return await _context.Qualidades
            .Where(x => x.Data.Date == data.Date)
            .OrderByDescending(x => x.Data)
            .ToListAsync();
    }

    public async Task<Qualidade?> GetByIdAsync(Guid id)
    {
        return await _context.Qualidades.FindAsync(id);
    }

    public async Task<IEnumerable<Qualidade>> GetByLinhaAsync(string linha)
    {
        return await _context.Qualidades
            .Where(x => x.Linha == linha)
            .OrderByDescending(x => x.Data)
            .ToListAsync();
    }

    public async Task<Qualidade> UpdateAsync(Qualidade entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Qualidades.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
