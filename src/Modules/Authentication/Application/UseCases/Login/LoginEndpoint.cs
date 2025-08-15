using Authentication.Domain.Users.Dtos;
using Carter;
using Core.Application;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Authentication.Application.UseCases.Login;

/// <summary>
/// Represents the request body for user login.
/// </summary>
/// <param name="Email">The user's email address.</param>
/// <param name="Password">The user's password.</param>
public record LoginRequest(
    string Email,
    string Password
);

/// <summary>
/// Represents the response returned after successful user login.
/// </summary>
/// <param name="User">The authenticated user information.</param>
/// <param name="Token">The JWT access token for the authenticated user.</param>
public record LoginResponse(
    UserResponseDto User,
    string Token
);

public class LoginEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(RouteGroup.Auth).WithTags("Authentication");
        
        group.MapPost("login", async (LoginRequest request, ISender sender) =>
            {
                // Send the command to log in the user
                var command = new LoginCommand(request.Email, request.Password);
                var result = await sender.Send(command);
            
                // Adapt the result to the response type
                var response = new LoginResponse(
                    result.AuthenticationResult.User,
                    result.AuthenticationResult.Token
                );
                
                return Results.Ok(response);
            })
            .WithName(RouteMetaField.Login.Name)
            .WithSummary(RouteMetaField.Login.Summary)
            .WithDescription(RouteMetaField.Login.Description)
            .ProducesValidationProblem()
            .Produces<LoginResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
        
        
    }
}