using Core.Application.Metadata;

namespace Authentication.Application.UseCases.Register;

/// <summary>
/// Contains metadata information for the user registration route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata Register = new(
        name: "RegisterUser",
        summary: "Register a new user account in the system.",
        description: """
             Creates a new user account with the provided email, password, and role.
             
             The email must be unique and in a valid format. The password must meet
             security requirements (minimum 8 characters with uppercase, lowercase, and digit).
             
             On success, returns a 201 Created response with user information and JWT token.
             
             If the email already exists, returns a 400 Bad Request response.
             
             If validation fails, returns a 400 Bad Request with validation errors.
         """
    );
}