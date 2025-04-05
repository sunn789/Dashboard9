
using System.Linq.Expressions;
using Modicom.Models.Entities;

namespace Modicom.Repo.Contracts;
public interface IContactUsRepository
{
    Task<IEnumerable<ContactUs>> GetAllAsync();
    Task<ContactUs?> GetByIdAsync(int id);
    Task AddAsync(ContactUs contactUs);
    Task UpdateAsync(ContactUs contactUs);
    Task DeleteAsync(int id);
    Task MarkAsReadAsync(int id);
}

