using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Modicom.Models;
using Modicom.Models.Entities;
using Modicom.Repo.Contracts;

public class SiteContentRepository : ISiteContentRepository
{
    private readonly DbContext _context;

    public SiteContentRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<SiteContent> GetByIdAsync(int id)
    {
        return await _context.Set<SiteContent>().FindAsync(id);
    }

    public async Task<IEnumerable<SiteContent>> GetAllAsync()
    {
        return await _context.Set<SiteContent>().ToListAsync();
    }

    public async Task<IEnumerable<SiteContent>> FindAsync(Expression<Func<SiteContent, bool>> predicate)
    {
        return await _context.Set<SiteContent>().Where(predicate).ToListAsync();
    }

    public async Task AddAsync(SiteContent entity)
    {
        await _context.Set<SiteContent>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SiteContent entity)
    {
        _context.Set<SiteContent>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Set<SiteContent>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}