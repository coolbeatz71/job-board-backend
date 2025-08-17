using FluentValidation;
using Job.Domain.Jobs.Enums;

namespace Job.Application.UseCases.GetAllJobs;

/// <summary>
/// Validator for <see cref="GetAllJobsQuery"/> that ensures the query parameters are valid.
/// </summary>
public class GetAllJobsValidator : AbstractValidator<GetAllJobsQuery>
{
    /// <summary>
    /// Configure validation rules for the query parameters.
    /// </summary>
    public GetAllJobsValidator()
    {
        RuleFor(x => x.PaginatedRequest.PageIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Page index must be greater than or equal to 0.");

        RuleFor(x => x.PaginatedRequest.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must not exceed 100.");

        RuleFor(x => x.Status)
            .Must(BeValidJobStatus)
            .WithMessage("Invalid job status. Valid values are: Active, Paused, Closed, Expired")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));

        RuleFor(x => x.JobType)
            .Must(BeValidJobType)
            .WithMessage("Invalid job type. Valid values are: FullTime, PartTime, Contract, Internship")
            .When(x => !string.IsNullOrWhiteSpace(x.JobType));

        RuleFor(x => x.WorkMode)
            .Must(BeValidWorkMode)
            .WithMessage("Invalid work mode. Valid values are: Remote, OnSite, Hybrid")
            .When(x => !string.IsNullOrWhiteSpace(x.WorkMode));

        RuleFor(x => x.Search)
            .MaximumLength(100)
            .WithMessage("Search term must not exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Search));
    }

    private static bool BeValidJobStatus(string? status)
    {
        return string.IsNullOrWhiteSpace(status) || Enum.TryParse<JobStatus>(status, true, out _);
    }

    private static bool BeValidJobType(string? jobType)
    {
        return string.IsNullOrWhiteSpace(jobType) || Enum.TryParse<JobType>(jobType, true, out _);
    }

    private static bool BeValidWorkMode(string? workMode)
    {
        return string.IsNullOrWhiteSpace(workMode) || Enum.TryParse<WorkMode>(workMode, true, out _);
    }
}