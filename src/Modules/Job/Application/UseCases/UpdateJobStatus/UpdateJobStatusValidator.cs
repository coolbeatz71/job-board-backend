using FluentValidation;
using Job.Domain.Jobs.Enums;

namespace Job.Application.UseCases.UpdateJobStatus;

/// <summary>
/// Validator for the UpdateJobStatus command to ensure valid input data.
/// </summary>
public class UpdateJobStatusValidator : AbstractValidator<UpdateJobStatusCommand>
{
    public UpdateJobStatusValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithMessage("Job ID is required.");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .Must(BeValidJobStatus)
            .WithMessage("Status must be one of: Active, Paused, Closed.");
    }

    /// <summary>
    /// Validates that the provided status is a valid job status.
    /// </summary>
    /// <param name="status">The status string to validate.</param>
    /// <returns>True if the status is valid, otherwise false.</returns>
    private static bool BeValidJobStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return false;

        return Enum.TryParse<JobStatus>(status, ignoreCase: true, out var jobStatus) &&
               jobStatus != JobStatus.Expired; // Expired cannot be set manually
    }
}