using MedgrupoChallenge.Domain.Entities;
using MedgrupoChallenge.Domain.Enums;

namespace MedgrupoChallenge.Application.DTOs;

public class ContactResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public int Age { get; set; }
    public bool IsActive { get; set; }

    public static ContactResponse FromEntity(Contact contact)
    {
        return new ContactResponse
        {
            Id = contact.Id,
            Name = contact.Name,
            BirthDate = contact.BirthDate,
            Gender = contact.Gender,
            Age = contact.Age,
            IsActive = contact.IsActive
        };
    }
}