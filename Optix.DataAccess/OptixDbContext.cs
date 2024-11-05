using Optix.DataAccess.DbModels;
using Optix.DataAccess.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Optix.DataAccess;

public class OptixDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    
    public OptixDbContext() : base()
    {
    }

    public OptixDbContext(DbContextOptions<OptixDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DataSeeder.SeedMovies(modelBuilder);
    }
}