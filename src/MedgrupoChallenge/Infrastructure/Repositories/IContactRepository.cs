using MedgrupoChallenge.Domain.Entities;

namespace MedgrupoChallenge.Infrastructure.Repositories;

public interface IContactRepository
{
    Task<Contact> AddAsync(Contact contact);
    Task<List<Contact>> GetAllActiveAsync();
    Task<Contact?> GetActiveByIdAsync(Guid id);
    Task<Contact?> GetByIdAsync(Guid id);
    Task UpdateAsync(Contact contact);
    Task DeleteAsync(Contact contact);
}