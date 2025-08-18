using Authentication.Application.Authorization;
using Carter;
using Core.Application;
using Job.Domain.Jobs.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Job.Application.UseCases.UpdateJobStatus;

public record UpdateJobStatusRequest(
    string Status
);

public record UpdateJobStatusResponse(JobResponseDto Job);

public class UpdateJobStatusEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(RouteGroup.Job).WithTags("Jobs");

        group.MapPatch("{id:guid}/status", async (Guid id, UpdateJobStatusRequest request, ISender sender) =>
        {
            var command = new UpdateJobStatusCommand(
                id,
                request.Status
            );

            var result = await sender.Send(command);
            var response = new UpdateJobStatusResponse(result.Job);

            return Results.Ok(response);
        })
        .WithName(RouteMetaField.UpdateJobStatus.Name)
        .WithSummary(RouteMetaField.UpdateJobStatus.Summary)
        .WithDescription(RouteMetaField.UpdateJobStatus.Description)
        .RequireAuthorization(AuthorizationPolicies.AdminOrEmployer)
        .ProducesValidationProblem()
        .Produces<UpdateJobStatusResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}