using System.Text;
using Authentication.Application.Authorization;
using Authentication.Application.Authorization.Handlers;
using Authentication.Application.Services;
using Authentication.Domain.Users.Enums;
using Authentication.Infrastructure;
using Authentication.Infrastructure.Seed;
using Core.Application.Configurations;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Interceptors;
using Core.Infrastructure.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Authentication;

/// <summary>
/// Provides extension methods to register and configure the Authentication module's services and middleware.
/// </summary>
public static class AuthenticationModule
{
    /// <summary>
    /// Adds the Authentication module's services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to register services into.</param>
    /// <param name="configuration">The application configuration instance.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
    /// <remarks>
    /// Registers MediatR handlers, database context with interceptors, and a data seeder.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.Services.AddAuthenticationModule(builder.Configuration);
    /// </code>
    /// </example>
    public static IServiceCollection AddAuthenticationModule(this IServiceCollection services, IConfiguration configuration)
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
        
        // Register AuthenticationDbContext with Postgres provider and naming convention.
        services.AddDbContextPool<AuthenticationDbContext>((serviceProvider, options) =>
        {
            options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });

        // Register data seeder for initial data population.
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IDataSeeder, AuthenticationDataSeeder>();
        services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();
        
        var (secret, issuer, audience, expiration) = AppEnvironment.Jwt();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        var policyRoleMap = new Dictionary<string, string[]>
        {
            { AuthorizationPolicies.AdminOnly, [nameof(UserRole.Admin)] },
            { AuthorizationPolicies.EmployerOnly, [nameof(UserRole.Employer)] },
            { AuthorizationPolicies.JobSeekerOnly, [nameof(UserRole.JobSeeker)] },
            { AuthorizationPolicies.AdminOrEmployer, [nameof(UserRole.Admin), nameof(UserRole.Employer)] },
            { 
                AuthorizationPolicies.AllRoles, 
                [
                    nameof(UserRole.Admin), nameof(UserRole.Employer), nameof(UserRole.JobSeeker)
                ]
            }
        };

        var authBuilder = services.AddAuthorizationBuilder();

        foreach (var (policyName, roles) in policyRoleMap)
        {
            authBuilder.AddPolicy(policyName, policy => 
                policy.Requirements.Add(new RoleRequirement(roles))
            );
        }
        
        return services;
    }
    
    /// <summary>
    /// Configures the Authentication module's middleware in the application pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The updated <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <remarks>
    /// Applies pending EF Core migrations and executes the data seeder.
    /// </remarks>
    /// <example>
    /// <code>
    /// app.UseAuthenticationModule();
    /// </code>
    /// </example>
    public static IApplicationBuilder UseAuthenticationModule(this IApplicationBuilder app)
    {
        // Configure Http request pipeline.
        // Use Api endpoint services.
        // Use application UseCase services.
        // Use DataSource - Infrastructure services.
        
        app.UseMigration<AuthenticationDbContext>();
        app.UseSeed();

        return app;
    }
}