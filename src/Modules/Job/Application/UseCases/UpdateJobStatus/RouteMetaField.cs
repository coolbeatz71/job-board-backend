using Core.Application.Metadata;

namespace Job.Application.UseCases.UpdateJobStatus;

/// <summary>
/// Contains metadata information for the job status update route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata UpdateJobStatus = new(
        name: "UpdateJobStatus",
        summary: "Update the status of an existing job listing.",
        description: """
             Updates the status of an existing job listing. Only employers and admins can update job status.
             
             Valid status values: Active, Paused, Closed
             
             On success, returns a 200 OK response with the updated job information.
             
             If validation fails, returns a 400 Bad Request with validation errors.
             
             If the job is not found, returns a 404 Not Found response.
             
             If unauthorized, returns a 401 Unauthorized response.
             
             If access denied, returns a 403 Forbidden response.
         """
    );
}