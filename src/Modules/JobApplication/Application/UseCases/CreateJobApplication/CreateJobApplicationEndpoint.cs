using System.Security.Claims;
using Authentication.Application.Authorization;
using Carter;
using Core.Application;
using JobApplication.Domain.JobApplications.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace JobApplication.Application.UseCases.CreateJobApplication;

/// <summary>
/// Represents the request body for creating a job application.
/// </summary>
/// <param name="CoverLetter">Optional cover letter content.</param>
/// <param name="ResumeUrl">The URL of the applicant's résumé.</param>
public record CreateJobApplicationRequest(
    string? CoverLetter,
    string ResumeUrl
);

/// <summary>
/// Represents the response returned after successful job application creation.
/// </summary>
/// <param name="JobApplication">The created job application information.</param>
public record CreateJobApplicationResponse(JobApplicationResponseDto JobApplication);

/// <summary>
/// Carter endpoint module for handling POST requests to create job applications.
/// </summary>
/// <remarks>
/// This endpoint requires the user to be authenticated as a JobSeeker.
/// The job ID comes from the URL parameter and the applicant ID is extracted from the JWT token.
/// </remarks>
public class CreateJobApplicationEndpoint : ICarterModule
{
    /// <summary>
    /// Configures the routes handled by this module.
    /// </summary>
    /// <param name="app">The endpoint route builder provided by ASP.NET Core.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(RouteGroup.Job).WithTags("Job Applications");

        group.MapPost("{jobId:guid}/apply", async (
                string jobId, 
                CreateJobApplicationRequest request, 
                ClaimsPrincipal user, ISender sender) =>
        {
            // Extract user ID from JWT token claims
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Results.BadRequest(new { error = "User not authenticated or invalid user ID." });
            }

            var command = new CreateJobApplicationCommand(
                jobId,
                userIdClaim,
                request.CoverLetter,
                request.ResumeUrl
            );

            var result = await sender.Send(command);
            var response = new CreateJobApplicationResponse(result.JobApplication);

            return Results.Created($"/{RouteGroup.Job}/{jobId}/applications/{response.JobApplication.Id}", response);
        })
        .WithName(RouteMetaField.CreateJobApplication.Name)
        .WithSummary(RouteMetaField.CreateJobApplication.Summary)
        .WithDescription(RouteMetaField.CreateJobApplication.Description)
        .RequireAuthorization(AuthorizationPolicies.JobSeekerOnly)
        .ProducesValidationProblem()
        .Produces<CreateJobApplicationResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}