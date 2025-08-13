using Core.Application.Configurations;
using Core.Infrastructure.Interceptors;
using Core.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Job;

/// <summary>
/// Provides extension methods to register and configure the Job module's services and middleware.
/// </summary>
public static class JobModule
{
    /// <summary>
    /// Adds the Job module's services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to register services into.</param>
    /// <param name="configuration">The application configuration instance.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
    /// <remarks>
    /// Registers MediatR handlers, database context with interceptors, and a data seeder.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.Services.AddJobModule(builder.Configuration);
    /// </code>
    /// </example>
    public static IServiceCollection AddJobModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        // Api Endpoint services.
        // Application UseCase services.
        // DataSource - Infrastructure services.
        
        // Read DB connection info from environment.
        var (port, db, user, pass) = AppEnvironment.Database();
        var connectionString = $"Host=127.0.0.1;Port={port};Database={db};Username={user};Password={pass};";
        
        // Register EF Core interceptors.
        services.AddSingleton<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddSingleton<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        
        // Register JobDbContext with Postgres provider and naming convention.
        services.AddDbContextPool<JobDbContext>((serviceProvider, options) =>
        {
            options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });

        // Register data seeder for initial data population.
        services.AddScoped<IDataSeeder, JobDataSeeder>();

        return services;

    }
}