using Carter;
using Core.Application;
using Job.Domain.Jobs.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Job.Application.UseCases.GetJob;

/// <summary>
/// Represents the response returned when retrieving a single job.
/// </summary>
/// <param name="Job">The job DTO associated with the requested ID.</param>
public record GetJobResponse(JobResponseDto Job);

/// <summary>
/// Carter endpoint module for handling GET requests to retrieve a single job by ID.
/// </summary>
/// <remarks>
/// This endpoint is publicly accessible and does not require authentication.
/// It retrieves a job by its unique identifier (GUID) and returns the job details.
/// </remarks>
public class GetJobEndpoint : ICarterModule
{
    /// <summary>
    /// Configures the routes handled by this module.
    /// </summary>
    /// <param name="app">The endpoint route builder provided by ASP.NET Core.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(RouteGroup.Job).WithTags("Jobs");

        group.MapGet("{id:guid}", async (string id, ISender sender) =>
        {
            // Send the query to retrieve the job
            var result = await sender.Send(new GetJobQuery(id));

            // Adapt the result to the HTTP response format
            var response = result.Adapt<GetJobResponse>();

            return Results.Ok(response);
        })
        .WithName(RouteMetaField.GetJob.Name)
        .WithSummary(RouteMetaField.GetJob.Summary)
        .WithDescription(RouteMetaField.GetJob.Description)
        .ProducesValidationProblem()
        .Produces<GetJobResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}