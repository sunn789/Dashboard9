

namespace Modicom.Repo.Contracts;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}