

using System.Linq.Expressions;

namespace Modicom.Repo.Contracts;

public interface IGenericRepository<T> where T : class
{
 Task<T> GetAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null); // Added for analytics
    IQueryable<T> GetQueryable(); // Added for complex queries
}