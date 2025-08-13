using Core.Domain;
using Job.Domain.Jobs.Entities;

namespace Job.Domain.Jobs.Events;

/// <summary>
/// Represents a domain event that is raised when an existing <see cref="JobEntity"/> is updated.
/// </summary>
/// <param name="JobEntity">The updated job entity.</param>
/// <remarks>
/// This event is typically published to notify other parts of the system that a job listing has been modified.
/// </remarks>
/// <example>
/// <code>
/// job.Update(...);
/// var domainEvent = new JobUpdatedEvent(job);
/// </code>
/// </example>
public record JobUpdatedEvent(JobEntity JobEntity): IDomainEvent;