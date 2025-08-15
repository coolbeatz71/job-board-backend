using Job.Domain.Jobs.Enums;

namespace Job.Domain.Jobs.ValueObjects;

public record WorkModeValue
{
    public WorkMode Value { get; init; }

    public WorkModeValue(WorkMode value)
    {
        if (!Enum.IsDefined(value)) throw new ArgumentException($"Invalid work mode: {value}");
        
        Value = value;
    }

    public WorkModeValue(string value)
    {
        if (!Enum.TryParse<WorkMode>(value, true, out var parsed) || !Enum.IsDefined(parsed))
        {
            throw new ArgumentException($"Invalid work mode: {value}");
        }

        Value = parsed;
    }

    public static implicit operator WorkMode(WorkModeValue workMode) => workMode.Value;
    public static implicit operator string(WorkModeValue workMode) => workMode.Value.ToString();
    public static implicit operator WorkModeValue(WorkMode workMode) => new(workMode);
    public static implicit operator WorkModeValue(string workMode) => new(workMode);
}