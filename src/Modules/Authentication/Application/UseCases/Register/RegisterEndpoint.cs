using Authentication.Domain.Users.Dtos;
using Authentication.Domain.Users.Enums;
using Authentication.Domain.Users.ValueObjects;
using Carter;
using Core.Application;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Authentication.Application.UseCases.Register;

/// <summary>
/// Represents the request body for user registration.
/// </summary>
/// <param name="Email">The user's email address.</param>
/// <param name="Password">The user's password.</param>
/// <param name="Role">The role to assign to the user (optional, defaults to job_seeker).</param>
public record RegisterRequest(
    string Email,
    string Password,
    string Role
);

/// <summary>
/// Represents the response returned after successful user registration.
/// </summary>
/// <param name="User">The registered user information.</param>
/// <param name="Token">The JWT access token for the authenticated user.</param>
public record RegisterResponse(
    UserResponseDto User,
    string Token
);

public class RegisterEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(RouteGroup.Auth).WithTags("Authentication");

        group.MapPost("register", async (RegisterRequest request, ISender sender) =>
        {
            // Send the command to register the user
            // var userRole = new Role(request.Role);
            var command = new RegisterCommand(request.Email, request.Password, request.Role);
            var result = await sender.Send(command);
            
            // Adapt the result to the response type
            var response = new RegisterResponse(
                result.AuthenticationResult.User,
                result.AuthenticationResult.Token
            );
                
            return Results.Created($"/{RouteGroup.User}/{response.User.Id}", response);
        })
        .WithName(RouteMetaField.Register.Name)
        .WithSummary(RouteMetaField.Register.Summary)
        .WithDescription(RouteMetaField.Register.Description)
        .ProducesValidationProblem()
        .Produces<RegisterResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}