using System.Reflection;
using Authentication.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure;

/// <summary>
/// Represents the Entity Framework database context for the authentication service.
/// </summary>
/// <param name="options">The options to configure the database context.</param>
public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options): DbContext(options)
{
    /// <summary>
    /// Gets the <see cref="DbSet{TEntity}"/> for accessing <see cref="UserEntity"/> records.
    /// </summary>
    public DbSet<UserEntity> Users => Set<UserEntity>();
    
    /// <summary>
    /// Configures the model for the context using Fluent API.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    /// <remarks>
    /// Sets the default schema to "authentication" and applies all entity configurations from the current assembly.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("authentication");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}