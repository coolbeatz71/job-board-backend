using Core.Application.CQRS;
using Job.Domain.Jobs.Dtos;

namespace Job.Application.UseCases.GetJob;

/// <summary>
/// Query used to retrieve a single job by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the job to retrieve.</param>
/// <remarks>
/// Example usage:
/// <code>
/// var query = new GetJobQuery("123e4567-e89b-12d3-a456-426614174000");
/// var result = await mediator.Send(query);
/// </code>
/// </remarks>
public record GetJobQuery(string Id) : IQuery<GetJobResult>;

/// <summary>
/// Result of the <see cref="GetJobQuery"/> containing the job details.
/// </summary>
/// <param name="Job">The job data for the requested ID.</param>
/// <remarks>
/// Returned by the handler upon successful retrieval of the job.
/// </remarks>
public record GetJobResult(JobResponseDto Job);