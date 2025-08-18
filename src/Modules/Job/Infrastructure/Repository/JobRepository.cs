using Job.Domain.Jobs.Entities;
using Job.Domain.Jobs.Repository;
using Microsoft.EntityFrameworkCore;

namespace Job.Infrastructure.Repository;

/// <summary>
/// Repository implementation for managing job entities using Entity Framework.
/// </summary>
public class JobRepository(JobDbContext context) : IJobRepository
{
    /// <inheritdoc />
    public async Task<JobEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Jobs.FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<JobEntity> CreateAsync(JobEntity job, CancellationToken cancellationToken = default)
    {
        context.Jobs.Add(job);
        await context.SaveChangesAsync(cancellationToken);
        return job;
    }

    /// <inheritdoc />
    public async Task<JobEntity> UpdateAsync(JobEntity job, CancellationToken cancellationToken = default)
    {
        context.Jobs.Update(job);
        await context.SaveChangesAsync(cancellationToken);
        return job;
    }
}