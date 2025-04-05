

using System.Linq.Expressions;

namespace Modicom.Repo.Contracts;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);
    Task<int> GetCountAsync(Expression<Func<T, bool>>? filter = null); // Added for analytics
    IQueryable<T> GetQueryable(); // Added for complex queries
    
}