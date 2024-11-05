using Optix.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Optix.Api.Startup;

public static class DataBaseMigration
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<OptixDbContext>(); 
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
        }
    }
}