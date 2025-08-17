using Core.Application.Metadata;

namespace Job.Application.UseCases.CreateJob;

/// <summary>
/// Contains metadata information for the job creation route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata CreateJob = new(
        name: "CreateJob",
        summary: "Create a new job listing in the system.",
        description: """
             Creates a new job listing with the provided details including title, description,
             company information, location, work mode, job type, and application deadline.
             
             On success, returns a 201 Created response with the created job information.
             
             If validation fails, returns a 400 Bad Request with validation errors.
             
             If unauthorized, returns a 401 Unauthorized response.
             
             If access denied, returns a 403 Forbidden response.
         """
    );
}