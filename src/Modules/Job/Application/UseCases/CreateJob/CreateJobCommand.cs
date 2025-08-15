using Core.Application.CQRS;
using Job.Domain.Jobs.Dtos;

namespace Job.Application.UseCases.CreateJob;

public record CreateJobCommand(
    string Title,
    string Description,
    string? Requirements,
    string CompanyName,
    string CompanyWebsite,
    string Location,
    string WorkMode,
    string Status,
    string JobType,
    DateTime? ApplicationDeadline
) : ICommand<CreateJobResult>;

public record CreateJobResult(JobResponseDto Job);