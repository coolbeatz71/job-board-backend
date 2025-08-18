using Core.Application.CQRS;
using Core.Application.Exceptions;
using Job.Domain.Jobs.Dtos;
using Job.Domain.Jobs.Repository;
using Job.Domain.Jobs.ValueObjects;
using Mapster;

namespace Job.Application.UseCases.UpdateJobStatus;

/// <summary>
/// Handles the UpdateJobStatus command to update a job's status.
/// </summary>
public class UpdateJobStatusHandler(IJobRepository jobRepository) : ICommandHandler<UpdateJobStatusCommand, UpdateJobStatusResult>
{
    /// <summary>
    /// Handles the UpdateJobStatus command.
    /// </summary>
    /// <param name="request">The command containing job ID and new status.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The result containing the updated job information.</returns>
    /// <exception cref="NotFoundException">Thrown when the job is not found.</exception>
    public async Task<UpdateJobStatusResult> Handle(UpdateJobStatusCommand request, CancellationToken cancellationToken)
    {
        // Get the existing job
        var existingJob = await jobRepository.GetByIdAsync(request.JobId, cancellationToken);
        if (existingJob == null)
        {
            throw new NotFoundException("Job", request.JobId);
        }

        // Parse the new status using value object
        var newStatus = new JobStatusValue(request.Status);

        // Update the job using the existing Update method
        var updatedJob = existingJob.Update(
            existingJob.Title,
            existingJob.Description,
            existingJob.Requirements,
            existingJob.CompanyName,
            existingJob.CompanyWebsite,
            existingJob.Location,
            existingJob.WorkMode,
            newStatus,
            existingJob.JobType,
            existingJob.ApplicationDeadline
        );

        // Save the changes
        await jobRepository.UpdateAsync(updatedJob, cancellationToken);

        // Map to DTO using Mapster
        var jobDto = updatedJob.Adapt<JobResponseDto>();
        return new UpdateJobStatusResult(jobDto);
    }
}