using Core.Application.CQRS;
using JobApplication.Domain.JobApplications.Dtos;

namespace JobApplication.Application.UseCases.UpdateJobApplicationStatus;

/// <summary>
/// Command to update the status of an existing job application.
/// </summary>
/// <param name="JobApplicationId">The unique identifier of the job application to update.</param>
/// <param name="Status">The new status to set for the job application.</param>
public record UpdateJobApplicationStatusCommand(
    Guid JobApplicationId,
    string Status
) : ICommand<UpdateJobApplicationStatusResult>;

/// <summary>
/// Result returned after successfully updating a job application status.
/// </summary>
/// <param name="JobApplication">The updated job application information.</param>
public record UpdateJobApplicationStatusResult(JobApplicationResponseDto JobApplication);