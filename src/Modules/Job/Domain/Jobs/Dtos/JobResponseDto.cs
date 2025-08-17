namespace Job.Domain.Jobs.Dtos;

public record JobResponseDto(
    Guid Id,
    string Title,
    string Description,
    string? Requirements,
    string CompanyName,
    string CompanyWebsite,
    string Location,
    string WorkMode,
    string Status,
    string JobType,
    DateTime? ApplicationDeadline,
    DateTime CreatedAt,
    DateTime UpdatedAt
);