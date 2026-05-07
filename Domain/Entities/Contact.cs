using MedgrupoChallenge.Domain.Enums;

namespace MedgrupoChallenge.Domain.Entities;

public class Contact
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateTime BirthDate { get; private set; }
    public Gender Gender { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdateAt { get; private set; }

    public int Age
    {
        get
        {
            var today = DateTime.Now;
            var age = today.Year - BirthDate.Year;

            if (BirthDate.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }

    public Contact()
    {
    }

    public Contact(string name, DateTime birthDate, Gender gender)
    {
        Validate(name, birthDate, gender);
        Name = name.Trim();
        BirthDate = birthDate.Date;
        Gender = gender;
        UpdateAt = DateTime.UtcNow;
    }

    public void Update(string name, DateTime birthDate, Gender gender)
    {
        Validate(name, birthDate, gender);
        Name = name.Trim();
        BirthDate = birthDate.Date;
        Gender = gender;
        UpdateAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if(!IsActive)
            throw new InvalidOperationException("Contact is already inactive.");

        IsActive = false;
        UpdateAt = DateTime.UtcNow;
    }
    
    private static void Validate(string name, DateTime birthDate, Gender gender)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Contact name is required.", nameof(name));

        if (birthDate.Date > DateTime.Today)
            throw new ArgumentException("Birth date cannot be greater than today's date.", nameof(birthDate));

        if (!Enum.IsDefined(typeof(Gender), gender))
            throw new ArgumentException("Invalid gender.", nameof(gender));

        var age = CalculateAge(birthDate);

        if (age == 0)
            throw new ArgumentException("Age cannot be equal to zero.", nameof(birthDate));

        if (age < 18)
            throw new ArgumentException("Contact must be at least 18 years old.", nameof(birthDate));
    }

    private static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}