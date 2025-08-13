using System.Reflection;
using Job.Domain.Jobs.Entities;
using Microsoft.EntityFrameworkCore;

namespace Job.Infrastructure;

/// <summary>
/// Represents the Entity Framework database context for the job service.
/// </summary>
/// <param name="options">The options to configure the database context.</param>
public class JobDbContext(DbContextOptions<JobDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets the <see cref="DbSet{TEntity}"/> for accessing <see cref="JobEntity"/> records.
    /// </summary>
    public DbSet<JobEntity> Jobs => Set<JobEntity>();
    
    /// <summary>
    /// Configures the model for the context using Fluent API.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    /// <remarks>
    /// Sets the default schema to "job" and applies all entity configurations from the current assembly.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("job");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}