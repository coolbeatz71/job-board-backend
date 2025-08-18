using Core.Application.CQRS;
using Job.Domain.Jobs.Dtos;

namespace Job.Application.UseCases.UpdateJobStatus;

/// <summary>
/// Command to update the status of an existing job.
/// </summary>
/// <param name="JobId">The unique identifier of the job to update.</param>
/// <param name="Status">The new status to set for the job (Active, Paused, Closed).</param>
public record UpdateJobStatusCommand(
    Guid JobId,
    string Status
) : ICommand<UpdateJobStatusResult>;

/// <summary>
/// Result returned after successfully updating a job status.
/// </summary>
/// <param name="Job">The updated job information.</param>
public record UpdateJobStatusResult(JobResponseDto Job);