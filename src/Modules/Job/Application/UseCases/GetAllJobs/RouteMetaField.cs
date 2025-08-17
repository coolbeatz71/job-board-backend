using Core.Application.Metadata;

namespace Job.Application.UseCases.GetAllJobs;

/// <summary>
/// Contains metadata information for the get all jobs route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata GetAllJobs = new(
        name: "GetAllJobs",
        summary: "Retrieve all job listings with optional filtering and pagination.",
        description: """
             Retrieves a paginated list of job listings with optional filters for status,
             job type, work mode, and search terms.
             
             This endpoint is publicly accessible and does not require authentication.
             
             Available filters:
             - Status: Active, Paused, Closed, Expired
             - JobType: FullTime, PartTime, Contract, Internship
             - WorkMode: Remote, OnSite, Hybrid
             - Search: Searches in title, description, and company name
             
             Pagination parameters:
             - PageIndex: Zero-based page index (default: 0)
             - PageSize: Number of items per page (default: 10, max: 100)
             
             On success, returns a 200 OK response with paginated job results including
             total count, current page index, page size, and job details.
             
             If validation fails, returns a 400 Bad Request with validation errors.
         """
    );
}