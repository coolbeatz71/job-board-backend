using Job.Domain.Jobs.Enums;

namespace Job.Domain.Jobs.ValueObjects;

public record JobStatusValue
{
    public JobStatus Value { get; init; }

    public JobStatusValue(JobStatus value)
    {
        if (!Enum.IsDefined(value)) throw new ArgumentException($"Invalid job status: {value}");
        
        Value = value;
    }

    public JobStatusValue(string value)
    {
        if (!Enum.TryParse<JobStatus>(value, true, out var parsed) || !Enum.IsDefined(parsed))
        {
            throw new ArgumentException($"Invalid job status: {value}");
        }

        Value = parsed;
    }

    public static implicit operator JobStatus(JobStatusValue jobStatus) => jobStatus.Value;
    public static implicit operator string(JobStatusValue jobStatus) => jobStatus.Value.ToString();
    public static implicit operator JobStatusValue(JobStatus jobStatus) => new(jobStatus);
    public static implicit operator JobStatusValue(string jobStatus) => new(jobStatus);
}