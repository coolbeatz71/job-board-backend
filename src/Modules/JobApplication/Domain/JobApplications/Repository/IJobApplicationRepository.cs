using Core.Domain;
using JobApplication.Domain.JobApplications.Entities;

namespace JobApplication.Domain.JobApplications.Repository;

/// <summary>
/// Repository interface for managing job application entities with CRUD operations.
/// </summary>
public interface IJobApplicationRepository : IRepository<JobApplicationEntity>
{
    /// <summary>
    /// Retrieves a job application by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the job application.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The job application entity if found, otherwise null.</returns>
    Task<JobApplicationEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new job application in the repository.
    /// </summary>
    /// <param name="jobApplication">The job application entity to create.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The created job application entity.</returns>
    Task<JobApplicationEntity> CreateAsync(JobApplicationEntity jobApplication, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing job application in the repository.
    /// </summary>
    /// <param name="jobApplication">The job application entity to update.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The updated job application entity.</returns>
    Task<JobApplicationEntity> UpdateAsync(JobApplicationEntity jobApplication, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a job application exists for the specified job and applicant.
    /// </summary>
    /// <param name="jobId">The job identifier.</param>
    /// <param name="applicantId">The applicant identifier.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>True if the application exists, otherwise false.</returns>
    Task<bool> ExistsAsync(Guid jobId, Guid applicantId, CancellationToken cancellationToken = default);
}