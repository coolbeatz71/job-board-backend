using Core.Application.Metadata;

namespace JobApplication.Application.UseCases.UpdateJobApplicationStatus;

/// <summary>
/// Contains metadata information for the job application status update route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata UpdateJobApplicationStatus = new(
        name: "UpdateJobApplicationStatus",
        summary: "Update the status of an existing job application.",
        description: """
             Updates the status of an existing job application. Only employers and admins can update application status.
             
             Valid status values: Submitted, UnderReview, Interviewed, Shortlisted, Rejected, Hired
             
             On success, returns a 200 OK response with the updated job application information.
             
             If validation fails, returns a 400 Bad Request with validation errors.
             
             If the job application is not found, returns a 404 Not Found response.
             
             If unauthorized, returns a 401 Unauthorized response.
             
             If access denied, returns a 403 Forbidden response.
         """
    );
}