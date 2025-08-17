using Job.Domain.Jobs.Entities;
using Job.Domain.Jobs.Repository;
using Microsoft.EntityFrameworkCore;

namespace Job.Infrastructure.Repository;

public class JobRepository(JobDbContext context) : IJobRepository
{
    public async Task<JobEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Jobs.FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
    }

    public async Task<JobEntity> CreateAsync(JobEntity job, CancellationToken cancellationToken = default)
    {
        context.Jobs.Add(job);
        await context.SaveChangesAsync(cancellationToken);
        return job;
    }
}