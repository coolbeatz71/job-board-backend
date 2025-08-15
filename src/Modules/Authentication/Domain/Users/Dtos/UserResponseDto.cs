namespace Authentication.Domain.Users.Dtos;

/// <summary>
/// Represents a user data transfer object containing basic user response.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="Email">The email address of the user.</param>
/// <param name="Role">The role assigned to the user.</param>
public record UserResponseDto(
    Guid Id,
    string Email,
    string Role
);