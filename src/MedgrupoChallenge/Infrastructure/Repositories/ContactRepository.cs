using MedgrupoChallenge.Domain.Entities;
using MedgrupoChallenge.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedgrupoChallenge.Infrastructure.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Contact> AddAsync(Contact contact)
    {
        await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();

        return contact;
    }

    public async Task<List<Contact>> GetAllActiveAsync()
    {
        return await _context.Contacts
            .AsNoTracking()
            .Where(contact => contact.IsActive)
            .OrderBy(contact => contact.Name)
            .ToListAsync();
    }

    public async Task<Contact?> GetActiveByIdAsync(Guid id)
    {
        return await _context.Contacts
            .FirstOrDefaultAsync(contact => contact.Id == id && contact.IsActive);
    }

    public async Task<Contact?> GetByIdAsync(Guid id)
    {
        return await _context.Contacts.FirstOrDefaultAsync(contact => contact.Id == id);
        
    }

    public async Task UpdateAsync(Contact contact)
    {
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Contact contact)
    {
        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
    }
}