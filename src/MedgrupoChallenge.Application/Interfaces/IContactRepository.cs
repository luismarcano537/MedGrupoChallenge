using MedgrupoChallenge.Domain.Entities;

namespace MedgrupoChallenge.Application.Interfaces;

public interface IContactRepository
{
    Task<IEnumerable<Contact>> GetAllActiveAsync();
    Task<Contact?> GetByIdAsync(Guid id);
    Task<Contact?> GetActiveByIdAsync(Guid id);
    Task<Contact> AddAsync(Contact contact);
    Task UpdateAsync(Contact contact);
    Task DeleteAsync(Contact contact);
}