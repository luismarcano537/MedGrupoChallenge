using MedgrupoChallenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedgrupoChallenge.Infrastructure.Data.Mappings;

public class ContactMapping : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");

        builder.HasKey(contact => contact.Id);

        builder.Property(contact => contact.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(contact => contact.BirthDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(contact => contact.Gender)
            .IsRequired();

        builder.Property(contact => contact.IsActive)
            .IsRequired();

        builder.Property(contact => contact.CreatedAt)
            .IsRequired();

        builder.Property(contact => contact.UpdatedAt);

        builder.Ignore(contact => contact.Age);
    }
}