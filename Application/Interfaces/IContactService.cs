using MedgrupoChallenge.Application.DTOs;

namespace MedgrupoChallenge.Application.Interfaces;

public interface IContactService
{
    Task<ContactResponse> CreateAsync(CreateContactRequest request);
    Task<List<ContactResponse>>  GetAllAsync();
    Task<ContactResponse?> GetByIdASync(Guid id);
    Task<ContactResponse?> UpdateAsync(Guid id, UpdateContactRequest request);
    Task<bool> DeactivateAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}