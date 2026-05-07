using Microsoft.EntityFrameworkCore;

namespace MedgrupoChallenge.Infraesctructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}