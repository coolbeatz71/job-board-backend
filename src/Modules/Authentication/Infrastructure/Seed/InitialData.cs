using Authentication.Application.Services;
using Authentication.Domain.Users.Entities;
using Authentication.Domain.Users.Enums;
using Core.Application.Configurations;

namespace Authentication.Infrastructure.Seed;

/// <summary>
/// Provides initial seed data for the <see cref="UserEntity"/> model.
/// </summary>
/// <remarks>
/// Creates exactly one user for each <see cref="UserRole"/> with fixed email addresses
/// and a password hash generated using <see cref="IPasswordService"/>.
/// </remarks>
public class InitialData
{
    /// <summary>
    /// Generates a collection of seed <see cref="UserEntity"/> instances.
    /// </summary>
    /// <param name="passwordService">
    /// The password service used to securely hash the plain-text password from the environment.
    /// </param>
    /// <returns>
    /// A collection of users representing each <see cref="UserRole"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the "DEFAULT_USER_PASSWORD" environment variable is not set or is empty.
    /// </exception>
    /// <example>
    /// <code>
    /// var users = InitialUserData.Users(passwordService);
    /// modelBuilder.Entity&lt;UserEntity&gt;().HasData(users);
    /// </code>
    /// </example>
    public static IEnumerable<UserEntity> Users(IPasswordService passwordService)
    {
        ArgumentNullException.ThrowIfNull(passwordService);

        var plainPassword = AppEnvironment.DefaultPassword();

        if (string.IsNullOrWhiteSpace(plainPassword))
        {
            throw new InvalidOperationException("DEFAULT_USER_PASSWORD env variable is missing or empty.");
        }
        
        var hashedPassword = passwordService.Hash(plainPassword);
        
        List<UserEntity> users = [
            UserEntity.Create(
                id: Guid.NewGuid(),
                email: "admin@jobboard.com",
                passwordHash: hashedPassword,
                role: UserRole.Admin
            ),
            UserEntity.Create(
                id: Guid.NewGuid(),
                email: "employer@jobboard.com",
                passwordHash: hashedPassword,
                role: UserRole.Employer
            ),
            UserEntity.Create(
                id: Guid.NewGuid(),
                email: "jobseeker@jobboard.com",
                passwordHash: hashedPassword,
                role: UserRole.JobSeeker
            )
        ];

        return users;
    }
}