using JobApplication.Domain.JobApplications.Entities;
using JobApplication.Domain.JobApplications.Repository;
using Microsoft.EntityFrameworkCore;

namespace JobApplication.Infrastructure.Repository;

public class JobApplicationRepository(JobApplicationDbContext context) : IJobApplicationRepository
{
    public async Task<JobApplicationEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.JobApplications.FirstOrDefaultAsync(ja => ja.Id == id, cancellationToken);
    }

    public async Task<JobApplicationEntity> CreateAsync(JobApplicationEntity jobApplication, CancellationToken cancellationToken = default)
    {
        context.JobApplications.Add(jobApplication);
        await context.SaveChangesAsync(cancellationToken);
        return jobApplication;
    }

    public async Task<JobApplicationEntity> UpdateAsync(JobApplicationEntity jobApplication, CancellationToken cancellationToken = default)
    {
        context.JobApplications.Update(jobApplication);
        await context.SaveChangesAsync(cancellationToken);
        return jobApplication;
    }

    public async Task<bool> ExistsAsync(Guid jobId, Guid applicantId, CancellationToken cancellationToken = default)
    {
        return await context.JobApplications
            .AnyAsync(ja => ja.JobId == jobId && ja.ApplicantId == applicantId, cancellationToken);
    }
}