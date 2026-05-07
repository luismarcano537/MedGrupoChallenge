using FluentAssertions;
using MedgrupoChallenge.Application.DTOs;
using MedgrupoChallenge.Application.Services;
using MedgrupoChallenge.Domain.Entities;
using MedgrupoChallenge.Domain.Enums;
using MedgrupoChallenge.Infrastructure.Repositories;
using Moq;

namespace MedgrupoChallenge.Tests.Application.Services;

public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _contactRepositoryMock;
    private readonly ContactService _contactService;

    public ContactServiceTests()
    {
        _contactRepositoryMock = new Mock<IContactRepository>();
        _contactService = new ContactService(_contactRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateContact_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateContactRequest
        {
            Name = "Luis Marcano",
            BirthDate = new DateTime(2000, 1, 1),
            Gender = Gender.Male
        };

        _contactRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Contact>()))
            .ReturnsAsync((Contact contact) => contact);

        // Act
        var result = await _contactService.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.BirthDate.Should().Be(request.BirthDate);
        result.Gender.Should().Be(request.Gender);
        result.IsActive.Should().BeTrue();
        result.Age.Should().BeGreaterThanOrEqualTo(18);

        _contactRepositoryMock.Verify(
            repository => repository.AddAsync(It.IsAny<Contact>()),
            Times.Once
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnContacts_WhenActiveContactsExist()
    {
        // Arrange
        var contacts = new List<Contact>
        {
            new("Luis Marcano", new DateTime(2000, 1, 1), Gender.Male),
            new("Maria Silva", new DateTime(1995, 5, 10), Gender.Female)
        };

        _contactRepositoryMock
            .Setup(repository => repository.GetAllActiveAsync())
            .ReturnsAsync(contacts);

        // Act
        var result = await _contactService.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(contact => contact.IsActive);

        _contactRepositoryMock.Verify(
            repository => repository.GetAllActiveAsync(),
            Times.Once
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoActiveContactsExist()
    {
        // Arrange
        _contactRepositoryMock
            .Setup(repository => repository.GetAllActiveAsync())
            .ReturnsAsync(new List<Contact>());

        // Act
        var result = await _contactService.GetAllAsync();

        // Assert
        result.Should().BeEmpty();

        _contactRepositoryMock.Verify(
            repository => repository.GetAllActiveAsync(),
            Times.Once
        );
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnContact_WhenActiveContactExists()
    {
        // Arrange
        var contact = new Contact(
            "Luis Marcano",
            new DateTime(2000, 1, 1),
            Gender.Male
        );

        _contactRepositoryMock
            .Setup(repository => repository.GetActiveByIdAsync(contact.Id))
            .ReturnsAsync(contact);

        // Act
        var result = await _contactService.GetByIdAsync(contact.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(contact.Id);
        result.Name.Should().Be(contact.Name);
        result.IsActive.Should().BeTrue();

        _contactRepositoryMock.Verify(
            repository => repository.GetActiveByIdAsync(contact.Id),
            Times.Once
        );
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenActiveContactDoesNotExist()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        _contactRepositoryMock
            .Setup(repository => repository.GetActiveByIdAsync(contactId))
            .ReturnsAsync((Contact?)null);

        // Act
        var result = await _contactService.GetByIdAsync(contactId);

        // Assert
        result.Should().BeNull();

        _contactRepositoryMock.Verify(
            repository => repository.GetActiveByIdAsync(contactId),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateContact_WhenActiveContactExists()
    {
        // Arrange
        var contact = new Contact(
            "Old Name",
            new DateTime(1990, 1, 1),
            Gender.Male
        );

        var request = new UpdateContactRequest
        {
            Name = "New Name",
            BirthDate = new DateTime(1992, 2, 2),
            Gender = Gender.Female
        };

        _contactRepositoryMock
            .Setup(repository => repository.GetActiveByIdAsync(contact.Id))
            .ReturnsAsync(contact);

        _contactRepositoryMock
            .Setup(repository => repository.UpdateAsync(contact))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _contactService.UpdateAsync(contact.Id, request);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(request.Name);
        result.BirthDate.Should().Be(request.BirthDate);
        result.Gender.Should().Be(request.Gender);

        _contactRepositoryMock.Verify(
            repository => repository.GetActiveByIdAsync(contact.Id),
            Times.Once
        );

        _contactRepositoryMock.Verify(
            repository => repository.UpdateAsync(contact),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenActiveContactDoesNotExist()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        var request = new UpdateContactRequest
        {
            Name = "New Name",
            BirthDate = new DateTime(1992, 2, 2),
            Gender = Gender.Female
        };

        _contactRepositoryMock
            .Setup(repository => repository.GetActiveByIdAsync(contactId))
            .ReturnsAsync((Contact?)null);

        // Act
        var result = await _contactService.UpdateAsync(contactId, request);

        // Assert
        result.Should().BeNull();

        _contactRepositoryMock.Verify(
            repository => repository.GetActiveByIdAsync(contactId),
            Times.Once
        );

        _contactRepositoryMock.Verify(
            repository => repository.UpdateAsync(It.IsAny<Contact>()),
            Times.Never
        );
    }

    [Fact]
    public async Task DeactivateAsync_ShouldDeactivateContact_WhenActiveContactExists()
    {
        // Arrange
        var contact = new Contact(
            "Active Contact",
            new DateTime(1990, 1, 1),
            Gender.Male
        );

        _contactRepositoryMock
            .Setup(repository => repository.GetActiveByIdAsync(contact.Id))
            .ReturnsAsync(contact);

        _contactRepositoryMock
            .Setup(repository => repository.UpdateAsync(contact))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _contactService.DeactivateAsync(contact.Id);

        // Assert
        result.Should().BeTrue();
        contact.IsActive.Should().BeFalse();

        _contactRepositoryMock.Verify(
            repository => repository.GetActiveByIdAsync(contact.Id),
            Times.Once
        );

        _contactRepositoryMock.Verify(
            repository => repository.UpdateAsync(contact),
            Times.Once
        );
    }

    [Fact]
    public async Task DeactivateAsync_ShouldReturnFalse_WhenActiveContactDoesNotExist()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        _contactRepositoryMock
            .Setup(repository => repository.GetActiveByIdAsync(contactId))
            .ReturnsAsync((Contact?)null);

        // Act
        var result = await _contactService.DeactivateAsync(contactId);

        // Assert
        result.Should().BeFalse();

        _contactRepositoryMock.Verify(
            repository => repository.GetActiveByIdAsync(contactId),
            Times.Once
        );

        _contactRepositoryMock.Verify(
            repository => repository.UpdateAsync(It.IsAny<Contact>()),
            Times.Never
        );
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteContact_WhenContactExists()
    {
        // Arrange
        var contact = new Contact(
            "Contact To Delete",
            new DateTime(1990, 1, 1),
            Gender.Male
        );

        _contactRepositoryMock
            .Setup(repository => repository.GetByIdAsync(contact.Id))
            .ReturnsAsync(contact);

        _contactRepositoryMock
            .Setup(repository => repository.DeleteAsync(contact))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _contactService.DeleteAsync(contact.Id);

        // Assert
        result.Should().BeTrue();

        _contactRepositoryMock.Verify(
            repository => repository.GetByIdAsync(contact.Id),
            Times.Once
        );

        _contactRepositoryMock.Verify(
            repository => repository.DeleteAsync(contact),
            Times.Once
        );
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenContactDoesNotExist()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        _contactRepositoryMock
            .Setup(repository => repository.GetByIdAsync(contactId))
            .ReturnsAsync((Contact?)null);

        // Act
        var result = await _contactService.DeleteAsync(contactId);

        // Assert
        result.Should().BeFalse();

        _contactRepositoryMock.Verify(
            repository => repository.GetByIdAsync(contactId),
            Times.Once
        );

        _contactRepositoryMock.Verify(
            repository => repository.DeleteAsync(It.IsAny<Contact>()),
            Times.Never
        );
    }
}