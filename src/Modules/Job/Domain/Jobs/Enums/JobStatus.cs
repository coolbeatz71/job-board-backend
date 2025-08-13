namespace Job.Domain.Jobs.Enums;

/// <summary>
/// Represents the current status of a job listing.
/// </summary>
public enum JobStatus
{
    Draft,
    Active,
    Paused,
    Closed,
    Expired
}