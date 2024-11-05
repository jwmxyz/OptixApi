using System.Reflection;
using Optix.Api.Factory;
using Optix.Api.Filters;
using Optix.Api.Responses;
using Optix.DataAccess;
using Microsoft.EntityFrameworkCore;
using Optix.DataAccess.DbModels;
using Optix.DataAccess.Repositories;
using Optix.Services;
using Optix.Services.Factories;
using Optix.Services.Interfaces;
using Optix.Services.Models.DTO;
using Optix.Services.SearchServices;
using Optix.Services.Validation;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Optix.Api.Startup;

public static class ServicesInitialisation
{
    public static void RegisterApplicationServices(this IServiceCollection services, WebApplicationBuilder builder) {
        RegisterCustomDependencies(services);
        RegisterDatabaseContextDependencies(services, builder);
    }
    
    private static void RegisterDatabaseContextDependencies(IServiceCollection services, WebApplicationBuilder builder)
    {
        var migrationsAssembly = typeof(OptixDbContext).GetTypeInfo().Assembly.GetName().Name;
        var mySqlConnectionStr = builder.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<OptixDbContext>(opt =>
        {
            opt.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr), sql => sql.MigrationsAssembly(migrationsAssembly));
            opt.UseMySql(ServerVersion.AutoDetect(mySqlConnectionStr), b => b.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, entity) => $"{schema ?? "dbo"}_{entity}"));
        });
    }
    
    private static void RegisterCustomDependencies(IServiceCollection services)
    {
        services
            .AddScoped<IResponseFactory, ResponseFactory>()
            .AddScoped<IRepository<Movie>, MovieRepository>()
            .AddScoped<IMovieSearchFactory, MovieSearchFactory>()
            .AddScoped<IMoveSearchService, MoveSearchService>()
            .AddScoped<IValidator<MovieSearchParams>, MovieSearchParamValidator>()
            .AddControllers(opt =>
            {
                opt.Filters.Add(typeof(OptixExceptionFilter));
            });
    }
}