using Core.Application.CQRS;
using Core.Application.Exceptions;
using JobApplication.Domain.JobApplications.Dtos;
using JobApplication.Domain.JobApplications.Enums;
using JobApplication.Domain.JobApplications.Repository;
using Mapster;

namespace JobApplication.Application.UseCases.UpdateJobApplicationStatus;

/// <summary>
/// Handles the UpdateJobApplicationStatus command to update a job application's status.
/// </summary>
public class UpdateJobApplicationStatusHandler(IJobApplicationRepository jobApplicationRepository) 
    : ICommandHandler<UpdateJobApplicationStatusCommand, UpdateJobApplicationStatusResult>
{
    /// <summary>
    /// Handles the UpdateJobApplicationStatus command.
    /// </summary>
    /// <param name="request">The command containing job application ID and new status.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The result containing the updated job application information.</returns>
    /// <exception cref="NotFoundException">Thrown when the job application is not found.</exception>
    public async Task<UpdateJobApplicationStatusResult> Handle(UpdateJobApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        // Get the existing job application
        var existingJobApplication = await jobApplicationRepository.GetByIdAsync(request.JobApplicationId, cancellationToken);
        if (existingJobApplication == null)
        {
            throw new NotFoundException("JobApplication", request.JobApplicationId);
        }

        // Parse the new status
        if (!Enum.TryParse<ApplicationStatus>(request.Status, ignoreCase: true, out var newStatus))
        {
            throw new BadRequestException($"Invalid application status: {request.Status}");
        }

        // Update the job application using the existing Update method
        var updatedJobApplication = existingJobApplication.Update(
            existingJobApplication.JobId,
            existingJobApplication.ApplicantId,
            newStatus,
            existingJobApplication.CoverLetter,
            existingJobApplication.ResumeUrl,
            existingJobApplication.Notes,
            existingJobApplication.ApplicationDate
        );

        // Save the changes
        await jobApplicationRepository.UpdateAsync(updatedJobApplication, cancellationToken);

        // Map to DTO using Mapster
        var jobApplicationDto = updatedJobApplication.Adapt<JobApplicationResponseDto>();
        return new UpdateJobApplicationStatusResult(jobApplicationDto);
    }
}