using Core.Application.Metadata;

namespace Authentication.Application.UseCases.Login;

/// <summary>
/// Contains metadata information for the user login route,
/// }such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata Login = new(
        name: "LoginUser",
        summary: "Authenticate a user and return a JWT token.",
        description: """
             Authenticates a user using their email and password credentials.

             The email must exist in the system and the password must match the stored hash.
             The user account must also be active to successfully authenticate.

             On success, returns a 200 OK response with user information and JWT token.

             If credentials are invalid, returns a 400 Bad Request response.
         """
    );
}