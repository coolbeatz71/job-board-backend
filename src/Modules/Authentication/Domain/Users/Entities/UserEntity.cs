using Authentication.Domain.Users.Enums;
using Core.Domain;

namespace Authentication.Domain.Users.Entities;

/// <summary>
/// Represents a user in the authentication system, containing credentials and role information.
/// </summary>
/// <remarks>
/// This entity serves as an aggregate root with a unique identifier, managing
/// essential authentication data such as email, password hash, and role.
/// It enforces validation for required fields and supports creation via
/// a static factory method.
/// </remarks>
public class UserEntity: Aggregate<Guid>
{
    /// <summary>
    /// Email address of the user. Must be unique.
    /// </summary>
    public string Email { get; private set; } = null!;
    
    /// <summary>
    /// Hashed password for secure authentication.
    /// </summary>
    public string PasswordHash { get; private set; } = null!;

    /// <summary>
    /// Role of the user (e.g., Admin, Employer, JobSeeker).
    /// </summary>
    public UserRole Role { get; private set; } = UserRole.JobSeeker;
    
    /// <summary>
    /// Creates a new <see cref="UserEntity"/> with the specified parameters.
    /// </summary>
    /// <param name="id">The unique identifier for the user.</param>
    /// <param name="email">The user's email address. Cannot be null or empty.</param>
    /// <param name="passwordHash">The hashed password for the user. Cannot be null or empty.</param>
    /// <param name="role">The role assigned to the user.</param>
    /// <returns>
    /// A new instance of <see cref="UserEntity"/> initialized with the provided values.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="email"/> or <paramref name="passwordHash"/> is null or empty.
    /// </exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var user = UserEntity.Create(
    ///     Guid.NewGuid(),
    ///     "john.doe@example.com",
    ///     passwordHasher.Hash("MyStrongPassword!"),
    ///     UserRole.Admin
    /// );
    /// ]]>
    /// </code>
    /// </example>
    public static UserEntity Create(
        Guid id,
        string email,
        string passwordHash,
        UserRole role
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(email);
        ArgumentException.ThrowIfNullOrEmpty(passwordHash);

        var user = new UserEntity
        {
            Id = id,
            Email = email,
            PasswordHash = passwordHash,
            Role = role
        };

        return user;
    }
}