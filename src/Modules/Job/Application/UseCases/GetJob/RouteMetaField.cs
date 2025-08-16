using Core.Application.Metadata;

namespace Job.Application.UseCases.GetJob;

/// <summary>
/// Contains metadata information for the get job route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata GetJob = new(
        name: "GetJob",
        summary: "Retrieve a single job by its unique identifier.",
        description: """
             Retrieves a job listing associated with the provided job ID.
             
             The ID must be a valid GUID format. This endpoint is publicly accessible
             and does not require authentication.
             
             On success, returns a 200 OK response with the job details including
             title, description, company information, location, work mode, job type,
             and application deadline.
             
             If the job is not found, returns a 404 Not Found response.
             
             If the ID format is invalid, returns a 400 Bad Request with validation errors.
         """
    );
}