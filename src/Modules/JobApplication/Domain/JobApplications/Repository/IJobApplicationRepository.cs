using Core.Domain;
using JobApplication.Domain.JobApplications.Entities;

namespace JobApplication.Domain.JobApplications.Repository;

public interface IJobApplicationRepository : IRepository<JobApplicationEntity>
{
    Task<JobApplicationEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<JobApplicationEntity> CreateAsync(JobApplicationEntity jobApplication, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid jobId, Guid applicantId, CancellationToken cancellationToken = default);
}