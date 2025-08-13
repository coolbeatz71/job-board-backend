using Core.Application.Configurations;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Interceptors;
using JobApplication.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobApplication;

/// <summary>
/// Provides extension methods to register and configure the Job Application module's services and middleware.
/// </summary>
public static class JobApplicationModule
{
    /// <summary>
    /// Adds the Job application module's services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to register services into.</param>
    /// <param name="configuration">The application configuration instance.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
    /// <remarks>
    /// Registers MediatR handlers, database context with interceptors, and a data seeder.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.Services.AddJobApplicationModule(builder.Configuration);
    /// </code>
    /// </example>
    public static IServiceCollection AddJobApplicationModule(this IServiceCollection services, IConfiguration configuration)
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
        
        // Register JobApplicationDbContext with Postgres provider and naming convention.
        services.AddDbContextPool<JobApplicationDbContext>((serviceProvider, options) =>
        {
            options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });
        
        return services;
    }
    
    /// <summary>
    /// Configures the Job Application module's middleware in the application pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The updated <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <remarks>
    /// Applies pending EF Core migrations and executes the data seeder.
    /// </remarks>
    /// <example>
    /// <code>
    /// app.UseApplicationJobModule();
    /// </code>
    /// </example>
    public static IApplicationBuilder UseJobApplicationModule(this IApplicationBuilder app)
    {
        // Configure Http request pipeline.
        // Use Api endpoint services.
        // Use application UseCase services.
        // Use DataSource - Infrastructure services.
        
        app.UseMigration<JobApplicationDbContext>();

        return app;
    }
}