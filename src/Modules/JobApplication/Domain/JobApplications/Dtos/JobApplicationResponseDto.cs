namespace JobApplication.Domain.JobApplications.Dtos;

public record JobApplicationResponseDto(
    Guid Id,
    Guid JobId,
    Guid ApplicantId,
    string Status,
    string? CoverLetter,
    string ResumeUrl,
    string? Notes,
    DateTime? ApplicationDate,
    DateTime CreatedAt,
    DateTime UpdatedAt
);