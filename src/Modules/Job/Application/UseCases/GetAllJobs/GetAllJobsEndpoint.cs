using Carter;
using Core.Application;
using Core.Application.Pagination;
using Job.Domain.Jobs.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Job.Application.UseCases.GetAllJobs;

/// <summary>
/// Represents the response returned when retrieving all jobs.
/// </summary>
/// <param name="Jobs">The paginated result containing job data and pagination metadata.</param>
public record GetAllJobsResponse(PaginatedResult<JobResponseDto> Jobs);

/// <summary>
/// Carter endpoint module for handling GET requests to retrieve all jobs with optional filtering and pagination.
/// </summary>
/// <remarks>
/// This endpoint is publicly accessible and does not require authentication.
/// It supports filtering by status, job type, work mode, and search terms, along with pagination.
/// </remarks>
public class GetAllJobsEndpoint : ICarterModule
{
    /// <summary>
    /// Configures the routes handled by this module.
    /// </summary>
    /// <param name="app">The endpoint route builder provided by ASP.NET Core.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(RouteGroup.Job).WithTags("Jobs");

        group.MapGet("", async (
            int pageIndex = 0,
            int pageSize = 10,
            string? status = null,
            string? jobType = null,
            string? workMode = null,
            string? search = null,
            ISender sender) =>
        {
            // Create pagination request
            var paginatedRequest = new PaginatedRequest(pageIndex, pageSize);

            // Send the query to retrieve all jobs
            var result = await sender.Send(new GetAllJobsQuery(
                paginatedRequest,
                status,
                jobType,
                workMode,
                search
            ));

            // Adapt the result to the HTTP response format
            var response = result.Adapt<GetAllJobsResponse>();

            return Results.Ok(response);
        })
        .WithName(RouteMetaField.GetAllJobs.Name)
        .WithSummary(RouteMetaField.GetAllJobs.Summary)
        .WithDescription(RouteMetaField.GetAllJobs.Description)
        .ProducesValidationProblem()
        .Produces<GetAllJobsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}