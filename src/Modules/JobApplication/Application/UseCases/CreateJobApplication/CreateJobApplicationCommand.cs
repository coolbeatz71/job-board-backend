using Core.Application.CQRS;
using JobApplication.Domain.JobApplications.Dtos;

namespace JobApplication.Application.UseCases.CreateJobApplication;

/// <summary>
/// Command used to create a new job application in the system.
/// </summary>
/// <param name="JobId">The unique identifier of the job being applied for (from URL parameter).</param>
/// <param name="ApplicantId">The unique identifier of the applicant (from JWT token claims).</param>
/// <param name="CoverLetter">Optional cover letter content.</param>
/// <param name="ResumeUrl">The URL of the applicant's résumé.</param>
/// <remarks>
/// The job ID comes from the URL parameter and the applicant ID is extracted from the JWT token claims.
/// Example usage:
/// <code>
/// var command = new CreateJobApplicationCommand(
///     jobId, // from URL parameter
///     applicantId, // from JWT token
///     "I am excited to apply for this position...",
///     "https://cdn.resume-storage.com/resume.pdf"
/// );
/// var result = await mediator.Send(command);
/// </code>
/// </remarks>
public record CreateJobApplicationCommand(
    string JobId,
    string ApplicantId,
    string? CoverLetter,
    string ResumeUrl
) : ICommand<CreateJobApplicationResult>;

/// <summary>
/// Result of the <see cref="CreateJobApplicationCommand"/> containing the job application details.
/// </summary>
/// <param name="JobApplication">The job application response DTO with application details.</param>
/// <remarks>
/// Returned by the handler upon successful job application creation.
/// </remarks>
public record CreateJobApplicationResult(JobApplicationResponseDto JobApplication);