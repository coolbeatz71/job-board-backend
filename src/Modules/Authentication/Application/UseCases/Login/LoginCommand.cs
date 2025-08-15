using Authentication.Domain.Users.Dtos;
using Core.Application.CQRS;

namespace Authentication.Application.UseCases.Login;

/// <summary>
/// Command used to authenticate a user with email and password.
/// </summary>
/// <param name="Email">The user's email address.</param>
/// <param name="Password">The user's password in plain text format.</param>
/// <remarks>
/// Example usage:
/// <code>
/// var command = new LoginCommand("user@example.com", "SecurePass123");
/// var result = await mediator.Send(command);
/// </code>
/// </remarks>
public record LoginCommand(
    string Email,
    string Password
) : ICommand<LoginResult>;

/// <summary>
/// Result of the <see cref="LoginCommand"/> containing the authentication details.
/// </summary>
/// <param name="AuthenticationResult">The authentication result with user info and JWT token.</param>
/// <remarks>
/// Returned by the handler upon successful user authentication.
/// </remarks>
public record LoginResult(AuthenticationResult AuthenticationResult);