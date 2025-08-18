using System.Security.Claims;
using Core.Application.CQRS;
using Core.Application.Exceptions;
using Core.Infrastructure.Extensions;
using Job.Domain.Jobs.Enums;
using Job.Domain.Jobs.Repository;
using JobApplication.Domain.JobApplications.Dtos;
using JobApplication.Domain.JobApplications.Entities;
using JobApplication.Domain.JobApplications.Enums;
using JobApplication.Domain.JobApplications.Repository;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace JobApplication.Application.UseCases.CreateJobApplication;

/// <summary>
/// Handles the <see cref="CreateJobApplicationCommand"/> by creating a new job application
/// after validating job availability and preventing duplicate applications.
/// </summary>
/// <param name="jobApplicationRepository">The repository for job application operations.</param>
/// <param name="jobRepository">The repository for job operations to validate job status.</param>
public class CreateJobApplicationHandler(
    IJobApplicationRepository jobApplicationRepository,
    IJobRepository jobRepository
) : ICommandHandler<CreateJobApplicationCommand, CreateJobApplicationResult>
{
    /// <summary>
    /// Handles the job application creation command with comprehensive validation.
    /// </summary>
    /// <param name="command">The job application creation command.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A <see cref="CreateJobApplicationResult"/> containing the created job application.</returns>
    /// <exception cref="NotFoundException">Thrown when the job with the specified ID is not found.</exception>
    /// <exception cref="BadRequestException">
    /// Thrown when:
    /// - The job is not in Active status (Paused, Closed, or Expired jobs cannot accept applications)
    /// - The applicant has already applied for this job
    /// - User is not authenticated or user ID claim is missing
    /// </exception>
    public async Task<CreateJobApplicationResult> Handle(CreateJobApplicationCommand command, CancellationToken cancellationToken)
    {
        var jobId = Guid.Parse(command.JobId);
        var applicantId = Guid.Parse(command.ApplicantId);
        
        // Validate that the job exists and get it
        var job = await jobRepository.GetByIdAsync(jobId, cancellationToken);
        if (job == null)
        {
            throw new NotFoundException($"Job with ID '{command.JobId}' was not found.");
        }

        // Validate that the job is active and accepting applications
        if (job.Status != JobStatus.Active)
        {
            var statusMessage = job.Status switch
            {
                JobStatus.Paused => "This job is currently paused and not accepting applications.",
                JobStatus.Closed => "This job is closed and no longer accepting applications.",
                JobStatus.Expired => "This job has expired and is no longer accepting applications.",
                _ => $"This job is in '{job.Status}' status and cannot accept applications."
            };
            throw new BadRequestException(statusMessage);
        }

        // Check if the applicant has already applied for this job
        var existingApplication = await jobApplicationRepository.ExistsAsync(jobId, applicantId, cancellationToken);
        if (existingApplication)
        {
            throw new BadRequestException("You have already applied for this job.");
        }

        // Create the job application
        var jobApplication = JobApplicationEntity.Create(
            id: Guid.NewGuid(),
            jobId: jobId,
            applicantId: applicantId,
            status: ApplicationStatus.Submitted,
            resumeUrl: command.ResumeUrl,
            coverLetter: command.CoverLetter,
            notes: null,
            applicationDate: DateTime.UtcNow
        );

        var createdJobApplication = await jobApplicationRepository.CreateAsync(jobApplication, cancellationToken);
        var jobApplicationDto = createdJobApplication.Adapt<JobApplicationResponseDto>();

        return new CreateJobApplicationResult(jobApplicationDto);
    }
}