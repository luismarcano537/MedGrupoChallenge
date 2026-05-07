using MedgrupoChallenge.Application.Interfaces;
using MedgrupoChallenge.Application.DTOs;
using MedgrupoChallenge.Domain.Entities;
using MedgrupoChallenge.Infraesctructure.Repositories;

namespace MedgrupoChallenge.Application.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<ContactResponse> CreateAsync(CreateContactRequest request)
    {
        var contact = new Contact(
            request.Name,
            request.BirthDate,
            request.Gender);

        var createdContact = await _contactRepository.AddAsync(contact);

        return ContactResponse.FromEntity(createdContact);
    }

    public async Task<List<ContactResponse>> GetAllAsync()
    {
        var contacts = await _contactRepository.GetAllActiveAsync();

        return contacts
            .Select(ContactResponse.FromEntity)
            .ToList();
    }

    public async Task<ContactResponse?> GetByIdASync(Guid id)
    {
        var contact = await _contactRepository.GetByIdAsync(id);

        if (contact is null)
            return null;

        return ContactResponse.FromEntity(contact);
    }

    public async Task<ContactResponse?> UpdateAsync(Guid id, UpdateContactRequest request)
    {
        var contact = await _contactRepository.GetByIdAsync(id);

        if (contact is null)
            return null;

        contact.Update(
            request.Name,
            request.BirthDate,
            request.Gender);

        await _contactRepository.UpdateAsync(contact);
        return ContactResponse.FromEntity(contact);
    }

    public async Task<bool> DeactivateAsync(Guid id)
    {
        var contact = await _contactRepository.GetActiveByIdAsync(id);

        if (contact is null)
            return false;

        contact.Deactivate();

        await _contactRepository.UpdateAsync(contact);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var contact = await _contactRepository.GetActiveByIdAsync(id);

        if (contact is null)
            return false;
        
        await _contactRepository.DeleteAsync(contact);
        return true;
    }
}