using Core.Domain;
using Job.Domain.Jobs.Entities;

namespace Job.Domain.Jobs.Events;

/// <summary>
/// Represents a domain event that is raised when a new <see cref="JobEntity"/> is created.
/// </summary>
/// <param name="JobEntity">The newly created job entity.</param>
/// <remarks>
/// This event is typically published to notify other parts of the system that a new job listing has been added to the job board.
/// </remarks>
/// <example>
/// <code>
/// var job = JobEntity.Create(...);
/// var domainEvent = new JobCreatedEvent(job);
/// </code>
/// </example>
public record JobCreatedEvent(JobEntity JobEntity): IDomainEvent;