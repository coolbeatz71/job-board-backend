using Core.Application.CQRS;
using Core.Application.Pagination;
using Job.Domain.Jobs.Dtos;
using Job.Domain.Jobs.Enums;
using Job.Infrastructure;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Job.Application.UseCases.GetAllJobs;

/// <summary>
/// Handles the <see cref="GetAllJobsQuery"/> by retrieving jobs from the database with optional filtering and pagination.
/// </summary>
/// <param name="jobDbContext">The <see cref="JobDbContext"/> used to query the job data.</param>
/// <remarks>
/// The handler uses EF Core's <c>Where</c> clause to filter jobs by status, job type, work mode, and search terms,
/// performs the query in a no-tracking context for read-only performance,
/// orders results by creation date (newest first), and maps entities to DTOs using Mapster.
/// </remarks>
/// <example>
/// <code>
/// var handler = new GetAllJobsHandler(jobDbContext);
/// var result = await handler.Handle(new GetAllJobsQuery(new PaginatedRequest(0, 10), Status: "Active"), CancellationToken.None);
/// </code>
/// </example>
public class GetAllJobsHandler(JobDbContext jobDbContext)
    : IQueryHandler<GetAllJobsQuery, GetAllJobsResult>
{
    /// <summary>
    /// Executes the query to retrieve jobs based on the provided filters and pagination.
    /// </summary>
    /// <param name="query">The job filter and pagination query.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the query if needed.</param>
    /// <returns>
    /// A <see cref="GetAllJobsResult"/> containing filtered and paginated jobs.
    /// </returns>
    public async Task<GetAllJobsResult> Handle(GetAllJobsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginatedRequest.PageIndex;
        var pageSize = query.PaginatedRequest.PageSize;
        
        var baseQuery = jobDbContext.Jobs.AsNoTracking();

        // Apply status filter
        if (!string.IsNullOrWhiteSpace(query.Status) && 
            Enum.TryParse<JobStatus>(query.Status, true, out var status))
        {
            baseQuery = baseQuery.Where(j => j.Status == status);
        }

        // Apply job type filter
        if (!string.IsNullOrWhiteSpace(query.JobType) && 
            Enum.TryParse<JobType>(query.JobType, true, out var jobType))
        {
            baseQuery = baseQuery.Where(j => j.JobType == jobType);
        }

        // Apply work mode filter
        if (!string.IsNullOrWhiteSpace(query.WorkMode) && 
            Enum.TryParse<WorkMode>(query.WorkMode, true, out var workMode))
        {
            baseQuery = baseQuery.Where(j => j.WorkMode == workMode);
        }

        // Apply search filter using case-insensitive matching
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            baseQuery = baseQuery.Where(j => 
                EF.Functions.ILike(j.Title, $"%{query.Search}%") ||
                EF.Functions.ILike(j.Description, $"%{query.Search}%") ||
                EF.Functions.ILike(j.CompanyName, $"%{query.Search}%")
            );
        }

        var count = await baseQuery.LongCountAsync(cancellationToken);
        
        // Get jobs with applied filters using dbContext
        var jobs = await baseQuery
            .OrderByDescending(j => j.CreatedAt)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        // Map job entity to JobResponseDto using Mapster
        var jobDtoList = jobs.Adapt<List<JobResponseDto>>();
        var paginatedResults = new PaginatedResult<JobResponseDto>(
            pageIndex, 
            pageSize, 
            count, 
            jobDtoList
        );
        
        // Return response
        return new GetAllJobsResult(paginatedResults);
    }
}