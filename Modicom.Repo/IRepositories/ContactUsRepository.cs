using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Modicom.Models;
using Modicom.Models.Entities;
using Modicom.Repo.Contracts;

public class ContactUsRepository : IContactUsRepository
{
    private readonly DbContext _context;

    public ContactUsRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ContactUs>> GetAllAsync()
    {
        return await _context.Set<ContactUs>().ToListAsync();
    }

    public async Task<ContactUs> GetByIdAsync(int id)
    {
        return await _context.Set<ContactUs>().FindAsync(id);
    }

    public async Task AddAsync(ContactUs contactUs)
    {
        await _context.Set<ContactUs>().AddAsync(contactUs);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ContactUs contactUs)
    {
        _context.Set<ContactUs>().Update(contactUs);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Set<ContactUs>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAsReadAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.ReadIt = true;
            _context.Set<ContactUs>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}