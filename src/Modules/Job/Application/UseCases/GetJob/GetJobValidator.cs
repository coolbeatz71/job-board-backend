using Core.Application.Extensions;
using FluentValidation;

namespace Job.Application.UseCases.GetJob;

/// <summary>
/// Validator for <see cref="GetJobQuery"/> that ensures the job data is valid.
/// </summary>
public class GetJobValidator : AbstractValidator<GetJobQuery>
{
    /// <summary>
    /// Configure validation rules for the job properties.
    /// </summary>
    public GetJobValidator()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .IsValidGuid("Job ID");
    }
}