using System.Linq.Expressions;

namespace Modicom.Repo.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Modicom.Models.Entities;

public interface ISiteContentRepository
{
    Task<SiteContent> GetByIdAsync(int id);
    Task<IEnumerable<SiteContent>> GetAllAsync();
    Task<IEnumerable<SiteContent>> FindAsync(Expression<Func<SiteContent, bool>> predicate);
    Task AddAsync(SiteContent entity);
    Task UpdateAsync(SiteContent entity);
    Task DeleteAsync(int id);
}


