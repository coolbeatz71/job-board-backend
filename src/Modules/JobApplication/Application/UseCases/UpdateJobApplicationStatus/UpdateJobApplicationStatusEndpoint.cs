using Authentication.Application.Authorization;
using Carter;
using Core.Application;
using JobApplication.Domain.JobApplications.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace JobApplication.Application.UseCases.UpdateJobApplicationStatus;

public record UpdateJobApplicationStatusRequest(
    string Status
);

public record UpdateJobApplicationStatusResponse(JobApplicationResponseDto JobApplication);

public class UpdateJobApplicationStatusEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(RouteGroup.JobApplication).WithTags("Job Applications");

        group.MapPatch("{id:guid}/status", async (Guid id, UpdateJobApplicationStatusRequest request, ISender sender) =>
        {
            var command = new UpdateJobApplicationStatusCommand(
                id,
                request.Status
            );

            var result = await sender.Send(command);
            var response = new UpdateJobApplicationStatusResponse(result.JobApplication);

            return Results.Ok(response);
        })
        .WithName(RouteMetaField.UpdateJobApplicationStatus.Name)
        .WithSummary(RouteMetaField.UpdateJobApplicationStatus.Summary)
        .WithDescription(RouteMetaField.UpdateJobApplicationStatus.Description)
        .RequireAuthorization(AuthorizationPolicies.AdminOrEmployer)
        .ProducesValidationProblem()
        .Produces<UpdateJobApplicationStatusResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}