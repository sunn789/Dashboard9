using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Modicom.Models;
using Modicom.Repo.Contracts;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    
    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<T> GetAsync(int id) => await _context.Set<T>().FindAsync(id);
    
    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
    
    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
    
    public async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
    }
    
    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await Task.CompletedTask;
    }
    
    public async Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null)
    {
        return filter != null 
            ? await _context.Set<T>().CountAsync(filter)
            : await _context.Set<T>().CountAsync();
    }
    
    public IQueryable<T> GetQueryable() => _context.Set<T>().AsQueryable();
}