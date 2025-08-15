using Authentication.Domain.Users.Dtos;
using Authentication.Domain.Users.Enums;
using Core.Application.CQRS;

namespace Authentication.Application.UseCases.Register;

/// <summary>
/// Command used to register a new user in the system.
/// </summary>
/// <param name="Email">The user's email address, which will serve as their username.</param>
/// <param name="Password">The user's password in plain text format.</param>
/// <param name="Role">The role to assign to the user (defaults to job_seeker).</param>
/// <remarks>
/// Example usage:
/// <code>
/// var command = new RegisterCommand("user@example.com", "SecurePass123", "job_seeker");
/// var result = await mediator.Send(command);
/// </code>
/// </remarks>
public record RegisterCommand(
    string Email,
    string Password,
    string Role
) : ICommand<RegisterResult>;

/// <summary>
/// Result of the <see cref="RegisterCommand"/> containing the authentication details.
/// </summary>
/// <param name="AuthenticationResult">The authentication result with user info and JWT token.</param>
/// <remarks>
/// Returned by the handler upon successful user registration and authentication.
/// </remarks>
public record RegisterResult(AuthenticationResult AuthenticationResult);