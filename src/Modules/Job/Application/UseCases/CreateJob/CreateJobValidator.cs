using FluentValidation;
using Job.Domain.Jobs.Enums;

namespace Job.Application.UseCases.CreateJob;

public class CreateJobValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(300)
            .WithMessage("Description must not exceed 300 characters");

        RuleFor(x => x.Requirements)
            .MaximumLength(300)
            .WithMessage("Requirements must not exceed 300 characters");

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage("Company name is required")
            .MaximumLength(100)
            .WithMessage("Company name must not exceed 100 characters");

        RuleFor(x => x.CompanyWebsite)
            .NotEmpty()
            .WithMessage("Company website is required")
            .MaximumLength(50)
            .WithMessage("Company website must not exceed 50 characters")
            .Must(BeValidUrl)
            .WithMessage("Company website must be a valid URL");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Location is required")
            .MaximumLength(50)
            .WithMessage("Location must not exceed 50 characters");

        RuleFor(x => x.WorkMode)
            .NotEmpty()
            .WithMessage("Work mode is required")
            .Must(BeValidWorkMode)
            .WithMessage("Invalid work mode. Valid values are: Remote, OnSite, Hybrid");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .Must(BeValidJobStatus)
            .WithMessage("Invalid job status. Valid values are: Active, Paused, Closed, Expired");

        RuleFor(x => x.JobType)
            .NotEmpty()
            .WithMessage("Job type is required")
            .Must(BeValidJobType)
            .WithMessage("Invalid job type. Valid values are: FullTime, PartTime, Contract, Internship");

        RuleFor(x => x.ApplicationDeadline)
            .Must(BeValidDeadline)
            .WithMessage("Application deadline must be in the future")
            .When(x => x.ApplicationDeadline.HasValue);
    }

    private static bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var result) && 
               (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }

    private static bool BeValidWorkMode(string workMode)
    {
        return Enum.TryParse<WorkMode>(workMode, true, out _);
    }

    private static bool BeValidJobStatus(string status)
    {
        return Enum.TryParse<JobStatus>(status, true, out _);
    }

    private static bool BeValidJobType(string jobType)
    {
        return Enum.TryParse<JobType>(jobType, true, out _);
    }

    private static bool BeValidDeadline(DateTime? deadline)
    {
        return !deadline.HasValue || deadline.Value > DateTime.UtcNow;
    }
}