using Core.Application.CQRS;
using Core.Application.Exceptions;
using Core.Infrastructure.Extensions;
using Job.Domain.Jobs.Dtos;
using Job.Infrastructure;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Job.Application.UseCases.GetJob;

/// <summary>
/// Handles the <see cref="GetJobQuery"/> to retrieve a single job by ID.
/// </summary>
/// <param name="jobDbContext">The database context for job operations.</param>
public class GetJobHandler(JobDbContext jobDbContext) : IQueryHandler<GetJobQuery, GetJobResult>
{
    /// <summary>
    /// Handles the query by fetching the job by ID and mapping it to a DTO.
    /// </summary>
    /// <param name="query">The query containing the job ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A <see cref="GetJobResult"/> containing the job DTO.</returns>
    /// <exception cref="NotFoundException">Thrown if no job is found with the specified ID.</exception>
    public async Task<GetJobResult> Handle(GetJobQuery query, CancellationToken cancellationToken)
    {
        var jobId = Guid.Parse(query.Id);
        
        var job = await jobDbContext.Jobs
            .Where(j => j.Id == jobId)
            .SingleDefaultOrThrowAsync(
                asNoTracking: true,
                keyName: "ID",
                keyValue: query.Id,
                cancellationToken: cancellationToken
            );
        
        var jobDto = job.Adapt<JobResponseDto>();
        
        return new GetJobResult(jobDto);
    }
}