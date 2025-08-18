using FluentValidation;
using JobApplication.Domain.JobApplications.Enums;

namespace JobApplication.Application.UseCases.UpdateJobApplicationStatus;

/// <summary>
/// Validator for the UpdateJobApplicationStatus command to ensure valid input data.
/// </summary>
public class UpdateJobApplicationStatusValidator : AbstractValidator<UpdateJobApplicationStatusCommand>
{
    public UpdateJobApplicationStatusValidator()
    {
        RuleFor(x => x.JobApplicationId)
            .NotEmpty()
            .WithMessage("Job Application ID is required.");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .Must(BeValidApplicationStatus)
            .WithMessage("Status must be one of: Submitted, UnderReview, Interviewed, Shortlisted, Rejected, Hired.");
    }

    /// <summary>
    /// Validates that the provided status is a valid application status.
    /// </summary>
    /// <param name="status">The status string to validate.</param>
    /// <returns>True if the status is valid, otherwise false.</returns>
    private static bool BeValidApplicationStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return false;

        return Enum.TryParse<ApplicationStatus>(status, ignoreCase: true, out _);
    }
}