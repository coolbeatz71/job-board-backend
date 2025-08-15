using Authentication.Domain.Users.Enums;
using FluentValidation;

namespace Authentication.Application.UseCases.Register;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    /// <summary>
    /// Configure validation rules for user registration.
    /// </summary>
    /// <remarks>
    /// Email must be valid format, password must meet security requirements,
    /// and a role must be one of the allowed values.
    /// </remarks>
    public RegisterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
            .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, and one digit.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => role != null && Enum.IsDefined(typeof(UserRole), role))
            .WithMessage("Role must be admin, employer, or job_seeker.");
    }
}