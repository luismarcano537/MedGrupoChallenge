using Microsoft.EntityFrameworkCore;
using MedgrupoChallenge.Domain.Entities;

namespace MedgrupoChallenge.Infraesctructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Contact> Contacts => Set<Contact>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable("Contacts");

            entity.HasKey(contact => contact.Id);

            entity.Property(contact => contact.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(contact => contact.BirthDate)
                .IsRequired()
                .HasColumnType("date");

            entity.Property(contact => contact.Gender)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(contact => contact.IsActive)
                .IsRequired();

            entity.Property(contact => contact.CreatedAt)
                .IsRequired();

            entity.Property(contact => contact.UpdatedAt)
                .IsRequired(false);

            entity.Ignore(contact => contact.Age);
        });
    }
}