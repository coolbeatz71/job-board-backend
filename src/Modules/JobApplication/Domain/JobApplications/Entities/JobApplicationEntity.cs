using System.ComponentModel.DataAnnotations;
using Core.Domain;
using JobApplication.Domain.JobApplications.Enums;
using JobApplication.Domain.JobApplications.Events;

namespace JobApplication.Domain.JobApplications.Entities;

/// <summary>
/// Represents an application submitted by a user for a specific job posting.
/// </summary>
/// <remarks>
/// This aggregate root manages job application details, tracks application status,
/// and raises domain events on creation and status updates.
/// </remarks>
public class JobApplicationEntity : Aggregate<Guid>
{
    /// <summary>
    /// Gets the unique identifier of the job for which this application was submitted.
    /// </summary>
    /// <value>A <see cref="Guid"/> representing the job listing identifier.</value>
    public Guid JobId { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the applicant user.
    /// </summary>
    /// <value>A <see cref="Guid"/> representing the applicant's user ID.</value>
    public Guid ApplicantId { get; private set; }

    /// <summary>
    /// Gets the current status of the application.
    /// </summary>
    /// <value>
    /// The application status such as <see cref="ApplicationStatus.Submitted"/>, 
    /// <see cref="ApplicationStatus.Shortlisted"/>, or <see cref="ApplicationStatus.Rejected"/>.
    /// Defaults to <see cref="ApplicationStatus.Submitted"/>.
    /// </value>
    public ApplicationStatus Status { get; private set; } = ApplicationStatus.Submitted;

    /// <summary>
    /// Gets the cover letter content submitted by the applicant.
    /// </summary>
    /// <value>
    /// The optional cover letter text. Can be null if no cover letter was provided.
    /// </value>
    [MaxLength(2000)]
    public string? CoverLetter { get; private set; }

    /// <summary>
    /// Gets the URL of the applicant's résumé or CV.
    /// </summary>
    /// <value>
    /// A non-null string containing a valid URL to the applicant's résumé.
    /// </value>
    [MaxLength(500)]
    public string ResumeUrl { get; private set; } = null!;

    /// <summary>
    /// Gets any additional internal notes related to the application.
    /// </summary>
    /// <value>
    /// Optional notes that may be added by recruiters or administrators.
    /// </value>
    [MaxLength(1000)]
    public string? Notes { get; private set; }

    /// <summary>
    /// Gets the date and time when the application was submitted.
    /// </summary>
    /// <value>
    /// A UTC <see cref="DateTime"/> value representing the submission time.
    /// Defaults to the current UTC date and time when created.
    /// </value>
    public DateTime? ApplicationDate { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a new <see cref="JobApplicationEntity"/> and raises a <see cref="JobApplicationCreatedEvent"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the application.</param>
    /// <param name="jobId">The identifier of the job being applied for. Cannot be empty.</param>
    /// <param name="applicantId">The identifier of the applicant. Cannot be empty.</param>
    /// <param name="status">The initial status of the application.</param>
    /// <param name="coverLetter">The optional cover letter text.</param>
    /// <param name="resumeUrl">The URL of the applicant's résumé. Cannot be null.</param>
    /// <param name="notes">Optional recruiter or admin notes.</param>
    /// <param name="applicationDate">The application submission date. Cannot be in the future.</param>
    /// <returns>A new instance of <see cref="JobApplicationEntity"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="jobId"/> or <paramref name="applicantId"/> is empty, 
    /// or when <paramref name="applicationDate"/> is in the future.
    /// </exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var application = JobApplicationEntity.Create(
    ///     Guid.NewGuid(),
    ///     jobId,
    ///     applicantId,
    ///     ApplicationStatus.Submitted,
    ///     "I am excited to apply for this position...",
    ///     "https://cdn.resume-storage.com/john_doe.pdf",
    ///     null,
    ///     DateTime.UtcNow
    /// );
    /// ]]>
    /// </code>
    /// </example>
    public static JobApplicationEntity Create(
        Guid id,
        Guid jobId,
        Guid applicantId,
        ApplicationStatus status,
        string? coverLetter,
        string resumeUrl,
        string? notes,
        DateTime? applicationDate
    )
    {
        if (jobId == Guid.Empty) throw new ArgumentException("JobId cannot be empty.");
        if (applicantId == Guid.Empty) throw new ArgumentException("ApplicantId cannot be empty.");
        
        if (applicationDate.HasValue && applicationDate.Value > DateTime.UtcNow)
        {
            throw new ArgumentException("Application date cannot be in the future.", nameof(applicationDate));
        }
        
        var application = new JobApplicationEntity
        {
            Id = id,
            JobId = jobId,
            ApplicantId = applicantId,
            Status = status,
            CoverLetter = coverLetter,
            ResumeUrl = resumeUrl,
            Notes = notes,
            ApplicationDate = applicationDate
        };
        
        application.AddDomainEvent(new JobApplicationCreatedEvent(application));
        return application;
    }

    /// <summary>
    /// Updates the properties of the application and raises a <see cref="JobApplicationStatusChangedEvent"/> if the status changes.
    /// </summary>
    /// <param name="jobId">The updated job ID. Cannot be empty.</param>
    /// <param name="applicantId">The updated applicant ID. Cannot be empty.</param>
    /// <param name="status">The updated application status.</param>
    /// <param name="coverLetter">The updated cover letter text. Can be null.</param>
    /// <param name="resumeUrl">The updated resume URL. Cannot be null.</param>
    /// <param name="notes">The updated notes. Can be null.</param>
    /// <param name="applicationDate">The updated application date. Cannot be in the future.</param>
    /// <returns>The current <see cref="JobApplicationEntity"/> instance with updated properties.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="jobId"/> or <paramref name="applicantId"/> is empty,
    /// or when <paramref name="applicationDate"/> is in the future.
    /// </exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// application.Update(
    ///     jobId,
    ///     applicantId,
    ///     ApplicationStatus.Shortlisted,
    ///     "Updated cover letter...",
    ///     "https://cdn.resume-storage.com/john_doe_updated.pdf",
    ///     "Reviewed by HR",
    ///     DateTime.UtcNow
    /// );
    /// ]]>
    /// </code>
    /// </example>
    public JobApplicationEntity Update(
        Guid jobId,
        Guid applicantId,
        ApplicationStatus status,
        string? coverLetter,
        string resumeUrl,
        string? notes,
        DateTime? applicationDate
    )
    {
        if (jobId == Guid.Empty) throw new ArgumentException("JobId cannot be empty.");
        if (applicantId == Guid.Empty) throw new ArgumentException("ApplicantId cannot be empty.");
        
        if (applicationDate.HasValue && applicationDate.Value > DateTime.UtcNow)
        {
            throw new ArgumentException("Application date cannot be in the future.", nameof(applicationDate));
        }

        JobId = jobId;
        ApplicantId = applicantId;
        CoverLetter = coverLetter;
        ResumeUrl = resumeUrl;
        Notes = notes;
        ApplicationDate = applicationDate;
        
        if (Status == status) return this;
        
        Status = status;
        AddDomainEvent(new JobApplicationStatusChangedEvent(this));
        return this;
    }
}
