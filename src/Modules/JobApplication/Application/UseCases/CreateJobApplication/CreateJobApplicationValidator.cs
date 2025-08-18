using Core.Application.Extensions;
using FluentValidation;

namespace JobApplication.Application.UseCases.CreateJobApplication;

/// <summary>
/// Validator for <see cref="CreateJobApplicationCommand"/> that ensures the job application data is valid.
/// </summary>
public class CreateJobApplicationValidator : AbstractValidator<CreateJobApplicationCommand>
{
    /// <summary>
    /// Configure validation rules for the job application properties.
    /// </summary>
    public CreateJobApplicationValidator()
    {
        RuleFor(x => x.JobId)
            .IsValidGuid("Job ID");

        RuleFor(x => x.ApplicantId)
            .IsValidGuid("Applicant ID");

        RuleFor(x => x.ResumeUrl)
            .NotEmpty()
            .WithMessage("Resume URL is required")
            .MaximumLength(500)
            .WithMessage("Resume URL must not exceed 500 characters")
            .Must(BeValidUrl)
            .WithMessage("Resume URL must be a valid URL");

        RuleFor(x => x.CoverLetter)
            .MaximumLength(2000)
            .WithMessage("Cover letter must not exceed 2000 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.CoverLetter));
    }

    private static bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var result) && 
               (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}