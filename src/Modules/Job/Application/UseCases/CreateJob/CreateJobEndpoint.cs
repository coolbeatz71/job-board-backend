using Authentication.Application.Authorization;
using Carter;
using Core.Application;
using Job.Domain.Jobs.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Job.Application.UseCases.CreateJob;

public record CreateJobRequest(
    string Title,
    string Description,
    string? Requirements,
    string CompanyName,
    string CompanyWebsite,
    string Location,
    string WorkMode,
    string Status,
    string JobType,
    DateTime? ApplicationDeadline
);

public record CreateJobResponse(JobResponseDto Job);

public class CreateJobEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(RouteGroup.Job).WithTags("Jobs");

        group.MapPost("", async (CreateJobRequest request, ISender sender) =>
        {
            var command = new CreateJobCommand(
                request.Title,
                request.Description,
                request.Requirements,
                request.CompanyName,
                request.CompanyWebsite,
                request.Location,
                request.WorkMode,
                request.Status,
                request.JobType,
                request.ApplicationDeadline
            );

            var result = await sender.Send(command);
            var response = new CreateJobResponse(result.Job);

            return Results.Created($"/{RouteGroup.Job}/{response.Job.Id}", response);
        })
        .WithName(RouteMetaField.CreateJob.Name)
        .WithSummary(RouteMetaField.CreateJob.Summary)
        .WithDescription(RouteMetaField.CreateJob.Description)
        .RequireAuthorization(AuthorizationPolicies.AdminOrEmployer)
        .ProducesValidationProblem()
        .Produces<CreateJobResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden);
    }
}