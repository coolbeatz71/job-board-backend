namespace Core.Application.Configurations;

/// <summary>
/// Provides access to environment-specific configuration values used in the application.
/// </summary>
public class AppEnvironment
{
    /// <summary>
    /// Retrieves postgresql database configuration values from environment variables.
    /// </summary>
    /// <remarks>
    /// This method is intended for development environments where connection details
    /// are provided via environment variables (e.g., from a .env file or local shell).
    ///
    /// Expected environment variables:
    /// - POSTGRES_PORT
    /// - POSTGRES_DB
    /// - POSTGRES_USER
    /// - POSTGRES_PASSWORD
    /// </remarks>
    /// <returns>
    /// A tuple containing:
    /// - <c>port</c>: The postgresql port (e.g., "5432")
    /// - <c>db</c>: The database name
    /// - <c>user</c>: The database username
    /// - <c>pass</c>: The database password
    /// </returns>
    public static (string? port, string? db, string? user, string? pass) Database()
    {
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var db   = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var pass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

        return (port, db, user, pass);
    }
    
    /// <summary>
    /// Retrieves the default password used for seeding or initializing user accounts.
    /// </summary>
    /// <remarks>
    /// The value is fetched from the <c>DEFAULT_USER_PASSWORD</c> environment variable.
    /// This is typically set in development or deployment environments to provide
    /// a consistent default password for newly created accounts during application setup.
    /// </remarks>
    /// <returns>
    /// The default password string, or <c>null</c> if the environment variable is not set.
    /// </returns>
    public static string? DefaultPassword()
    {
        var defaultPassword = Environment.GetEnvironmentVariable("DEFAULT_USER_PASSWORD");
        return defaultPassword;
    }
}