namespace Job.Domain.Jobs.Enums;

/// <summary>
/// Represents the current status of a job listing.
/// </summary>
public enum JobStatus
{
    Active,
    Paused,
    Closed,
    Expired
}