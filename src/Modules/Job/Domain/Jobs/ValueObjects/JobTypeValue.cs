using Job.Domain.Jobs.Enums;

namespace Job.Domain.Jobs.ValueObjects;

public record JobTypeValue
{
    public JobType Value { get; init; }

    public JobTypeValue(JobType value)
    {
        if (!Enum.IsDefined(value)) throw new ArgumentException($"Invalid job type: {value}");
        
        Value = value;
    }

    public JobTypeValue(string value)
    {
        if (!Enum.TryParse<JobType>(value, true, out var parsed) || !Enum.IsDefined(parsed))
        {
            throw new ArgumentException($"Invalid job type: {value}");
        }

        Value = parsed;
    }

    public static implicit operator JobType(JobTypeValue jobType) => jobType.Value;
    public static implicit operator string(JobTypeValue jobType) => jobType.Value.ToString();
    public static implicit operator JobTypeValue(JobType jobType) => new(jobType);
    public static implicit operator JobTypeValue(string jobType) => new(jobType);
}