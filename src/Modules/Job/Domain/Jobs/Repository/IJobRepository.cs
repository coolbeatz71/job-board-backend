using Core.Domain;
using Job.Domain.Jobs.Entities;

namespace Job.Domain.Jobs.Repository;

/// <summary>
/// Repository interface for managing job entities with basic CRUD operations.
/// </summary>
public interface IJobRepository : IRepository<JobEntity>
{
    /// <summary>
    /// Retrieves a job by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the job.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The job entity if found, otherwise null.</returns>
    Task<JobEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new job in the repository.
    /// </summary>
    /// <param name="job">The job entity to create.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The created job entity.</returns>
    Task<JobEntity> CreateAsync(JobEntity job, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing job in the repository.
    /// </summary>
    /// <param name="job">The job entity to update.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The updated job entity.</returns>
    Task<JobEntity> UpdateAsync(JobEntity job, CancellationToken cancellationToken = default);
}