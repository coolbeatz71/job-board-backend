namespace Authentication.Domain.Users.Dtos;

/// <summary>
/// Represents the result of an authentication operation.
/// </summary>
/// <param name="User">The authenticated user information.</param>
/// <param name="Token">The JWT access token for the authenticated user.</param>
public record AuthenticationResult(
    UserDto User,
    string Token
);