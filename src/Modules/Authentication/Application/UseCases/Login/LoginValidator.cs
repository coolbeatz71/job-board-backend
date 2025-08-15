using FluentValidation;

namespace Authentication.Application.UseCases.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    /// <summary>
    /// Configure validation rules for user login.
    /// </summary>
    /// <remarks>
    /// Email must be in valid format and password must be provided.
    /// </remarks>
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}