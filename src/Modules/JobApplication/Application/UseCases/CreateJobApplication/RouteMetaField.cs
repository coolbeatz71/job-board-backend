using Core.Application.Metadata;

namespace JobApplication.Application.UseCases.CreateJobApplication;

/// <summary>
/// Contains metadata information for the create job application route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata CreateJobApplication = new(
        name: "CreateJobApplication",
        summary: "Apply for a job position.",
        description: """
             Creates a new job application for the specified job position.
             
             Only accessible by authenticated JobSeekers. The job ID is provided in the URL
             and the applicant ID is automatically extracted from the JWT token.
             
             Validates that:
             - The job exists and is in Active status
             - The applicant hasn't already applied for this job
             - The resume URL is valid
             
             Jobs that are Paused, Closed, or Expired cannot accept new applications.
             
             On success, returns a 201 Created response with the job application details.
             
             If the job is not found, returns a 404 Not Found response.
             
             If the job is not accepting applications, returns a 400 Bad Request with explanation.
             
             If the user has already applied, returns a 400 Bad Request response.
             
             If validation fails, returns a 400 Bad Request with validation errors.
             
             If unauthorized, returns a 401 Unauthorized response.
             
             If access denied (not a JobSeeker), returns a 403 Forbidden response.
         """
    );
}