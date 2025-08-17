using Core.Domain;
using Job.Domain.Jobs.Entities;

namespace Job.Domain.Jobs.Repository;

public interface IJobRepository : IRepository<JobEntity>
{
    Task<JobEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<JobEntity> CreateAsync(JobEntity job, CancellationToken cancellationToken = default);
}