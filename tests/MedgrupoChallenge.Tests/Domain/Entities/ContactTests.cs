using FluentAssertions;
using MedgrupoChallenge.Domain.Entities;
using MedgrupoChallenge.Domain.Enums;

namespace MedgrupoChallenge.Tests.Domain.Entities;

public class ContactTests
{
    [Fact]
    public void Constructor_ShouldCreateContact_WhenDataIsValid()
    {
        // Arrange
        var name = "Luis Marcano";
        var birthDate = new DateTime(2000, 1, 1);
        var gender = Gender.Male;

        // Act
        var contact = new Contact(name, birthDate, gender);

        // Assert
        contact.Id.Should().NotBeEmpty();
        contact.Name.Should().Be(name);
        contact.BirthDate.Should().Be(birthDate);
        contact.Gender.Should().Be(gender);
        contact.IsActive.Should().BeTrue();
        contact.CreatedAt.Should().NotBe(default);
        contact.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Constructor_ShouldTrimContactName_WhenNameHasExtraSpaces()
    {
        // Arrange
        var name = "  Luis Marcano  ";
        var birthDate = new DateTime(2000, 1, 1);
        var gender = Gender.Male;

        // Act
        var contact = new Contact(name, birthDate, gender);

        // Assert
        contact.Name.Should().Be("Luis Marcano");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsEmptyOrWhiteSpace(string invalidName)
    {
        // Arrange
        var birthDate = new DateTime(1990, 1, 1);
        var gender = Gender.Male;

        // Act
        var act = () => new Contact(invalidName, birthDate, gender);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Contact name is required.*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsNull()
    {
        // Arrange
        string? name = null;
        var birthDate = new DateTime(1990, 1, 1);
        var gender = Gender.Male;

        // Act
        var act = () => new Contact(name!, birthDate, gender);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Contact name is required.*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenBirthDateIsInFuture()
    {
        // Arrange
        var name = "Future Contact";
        var birthDate = DateTime.Today.AddDays(1);
        var gender = Gender.Male;

        // Act
        var act = () => new Contact(name, birthDate, gender);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Birth date cannot be greater than today's date.*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenAgeIsZero()
    {
        // Arrange
        var name = "Zero Age Contact";
        var birthDate = DateTime.Today;
        var gender = Gender.Male;

        // Act
        var act = () => new Contact(name, birthDate, gender);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Age cannot be equal to zero.*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenContactIsUnderage()
    {
        // Arrange
        var name = "Underage Contact";
        var birthDate = DateTime.Today.AddYears(-17);
        var gender = Gender.Male;

        // Act
        var act = () => new Contact(name, birthDate, gender);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Contact must be at least 18 years old.*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenGenderIsInvalid()
    {
        // Arrange
        var name = "Invalid Gender Contact";
        var birthDate = new DateTime(1990, 1, 1);
        var invalidGender = (Gender)999;

        // Act
        var act = () => new Contact(name, birthDate, invalidGender);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Invalid gender.*");
    }

    [Fact]
    public void Age_ShouldReturnCorrectAge_WhenBirthdayAlreadyOccurredThisYear()
    {
        // Arrange
        var expectedAge = 30;
        var birthDate = DateTime.Today.AddYears(-expectedAge).AddDays(-1);

        var contact = new Contact(
            "Age Test",
            birthDate,
            Gender.Male
        );

        // Act
        var age = contact.Age;

        // Assert
        age.Should().Be(expectedAge);
    }

    [Fact]
    public void Age_ShouldReturnCorrectAge_WhenBirthdayIsToday()
    {
        // Arrange
        var expectedAge = 30;
        var birthDate = DateTime.Today.AddYears(-expectedAge);

        var contact = new Contact(
            "Birthday Today Test",
            birthDate,
            Gender.Male
        );

        // Act
        var age = contact.Age;

        // Assert
        age.Should().Be(expectedAge);
    }

    [Fact]
    public void Age_ShouldReturnCorrectAge_WhenBirthdayHasNotOccurredThisYear()
    {
        // Arrange
        var birthDate = DateTime.Today.AddYears(-30).AddDays(1);

        var contact = new Contact(
            "Age Test",
            birthDate,
            Gender.Male
        );

        // Act
        var age = contact.Age;

        // Assert
        age.Should().Be(29);
    }

    [Fact]
    public void Update_ShouldChangeContactData_WhenDataIsValid()
    {
        // Arrange
        var contact = new Contact(
            "Old Name",
            new DateTime(1990, 1, 1),
            Gender.Male
        );

        var newName = "New Name";
        var newBirthDate = new DateTime(1992, 2, 2);
        var newGender = Gender.Female;

        // Act
        contact.Update(newName, newBirthDate, newGender);

        // Assert
        contact.Name.Should().Be(newName);
        contact.BirthDate.Should().Be(newBirthDate);
        contact.Gender.Should().Be(newGender);
        contact.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Update_ShouldTrimContactName_WhenNameHasExtraSpaces()
    {
        // Arrange
        var contact = new Contact(
            "Old Name",
            new DateTime(1990, 1, 1),
            Gender.Male
        );

        // Act
        contact.Update(
            "  New Name  ",
            new DateTime(1992, 2, 2),
            Gender.Female
        );

        // Assert
        contact.Name.Should().Be("New Name");
    }

    [Fact]
    public void Update_ShouldThrowArgumentException_WhenUpdatedDataIsInvalid()
    {
        // Arrange
        var contact = new Contact(
            "Valid Name",
            new DateTime(1990, 1, 1),
            Gender.Male
        );

        // Act
        var act = () => contact.Update(
            "",
            new DateTime(1992, 2, 2),
            Gender.Female
        );

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Contact name is required.*");
    }

    [Fact]
    public void Deactivate_ShouldSetContactAsInactive_WhenContactIsActive()
    {
        // Arrange
        var contact = new Contact(
            "Active Contact",
            new DateTime(1990, 1, 1),
            Gender.Male
        );

        // Act
        contact.Deactivate();

        // Assert
        contact.IsActive.Should().BeFalse();
        contact.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Deactivate_ShouldThrowInvalidOperationException_WhenContactIsAlreadyInactive()
    {
        // Arrange
        var contact = new Contact(
            "Inactive Contact",
            new DateTime(1990, 1, 1),
            Gender.Male
        );

        contact.Deactivate();

        // Act
        var act = () => contact.Deactivate();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Contact is already inactive.");
    }
}