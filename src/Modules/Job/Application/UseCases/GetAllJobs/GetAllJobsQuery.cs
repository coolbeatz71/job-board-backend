using Core.Application.CQRS;
using Core.Application.Pagination;
using Job.Domain.Jobs.Dtos;

namespace Job.Application.UseCases.GetAllJobs;

/// <summary>
/// Query to retrieve all jobs with optional filtering and pagination support.
/// </summary>
/// <param name="PaginatedRequest">Pagination parameters used to control page size and index.</param>
/// <param name="Status">Optional filter by job status (Active, Paused, Closed, Expired).</param>
/// <param name="JobType">Optional filter by job type (FullTime, PartTime, Contract, Internship).</param>
/// <param name="WorkMode">Optional filter by work mode (Remote, OnSite, Hybrid).</param>
/// <param name="Search">Optional search term to filter by title, description, or company name.</param>
/// <remarks>
/// This query enables filtering of job listings with pagination support.
/// </remarks>
/// <example>
/// <code>
/// var query = new GetAllJobsQuery(
///     new PaginatedRequest(pageIndex: 0, pageSize: 20), 
///     Status: "Active", 
///     JobType: "FullTime",
///     WorkMode: "Remote",
///     Search: "software developer"
/// );
/// var result = await mediator.Send(query);
/// </code>
/// </example>
public record GetAllJobsQuery(
    PaginatedRequest PaginatedRequest,
    string? Status = null,
    string? JobType = null,
    string? WorkMode = null,
    string? Search = null
) : IQuery<GetAllJobsResult>;

/// <summary>
/// The response containing a paginated list of jobs with applied filters.
/// </summary>
/// <param name="Jobs">
/// A paginated result containing job DTOs that match the specified filters.
/// Includes metadata like total item count and current page index.
/// </param>
/// <example>
/// <code>
/// var response = new GetAllJobsResult(paginatedJobs);
/// </code>
/// </example>
public record GetAllJobsResult(PaginatedResult<JobResponseDto> Jobs);