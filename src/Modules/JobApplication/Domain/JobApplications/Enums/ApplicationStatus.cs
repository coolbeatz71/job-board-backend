namespace JobApplication.Domain.JobApplications.Enums;

/// <summary>
/// Represents the status of a job application.
/// </summary>
public enum ApplicationStatus
{
    Submitted,
    UnderReview,
    Interviewed,
    Shortlisted,
    Rejected,
    Hired,
}