using MedgrupoChallenge.Domain.Enums;

namespace MedgrupoChallenge.Application.DTOs;

public class CreateContactRequest
{
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
}