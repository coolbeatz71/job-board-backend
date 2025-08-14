using System.Reflection;
using Authentication.Domain.Users.Entities;
using Job.Domain.Jobs.Entities;
using JobApplication.Domain.JobApplications.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobApplication.Infrastructure;

/// <summary>
/// Represents the Entity Framework database context for the Job application module.
/// </summary>
public class JobApplicationDbContext(DbContextOptions<JobApplicationDbContext> options): DbContext(options)
{
    /// <summary>
    /// Gets the <see cref="DbSet{TEntity}"/> for <see cref="JobApplicationEntity"/>.
    /// </summary>
    public DbSet<JobApplicationEntity> JobApplications => Set<JobApplicationEntity>();
    
    /// <summary>
    /// Gets the <see cref="DbSet{TEntity}"/> for <see cref="JobEntity"/>.
    /// </summary>
    public DbSet<JobEntity> Jobs => Set<JobEntity>();
    
    /// <summary>
    /// Gets the <see cref="DbSet{TEntity}"/> for <see cref="UserEntity"/>.
    /// </summary>
    public DbSet<UserEntity> Users => Set<UserEntity>();
    
    /// <summary>
    /// Configures the model for the context using Fluent API.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    /// <remarks>
    /// Sets the default schema to "application" and applies all entity configurations from the current assembly.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("application");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}