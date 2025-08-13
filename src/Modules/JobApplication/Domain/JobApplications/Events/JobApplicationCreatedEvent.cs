using Core.Domain;
using JobApplication.Domain.JobApplications.Entities;

namespace JobApplication.Domain.JobApplications.Events;

/// <summary>
/// Represents a domain event that is raised when a new <see cref="JobApplicationEntity"/> is created.
/// </summary>
/// <param name="JobApplicationEntity">The newly created job application entity.</param>
/// <remarks>
/// This event is typically published to notify other parts of the system that a new application was created
/// for a particular job.
/// </remarks>
/// <example>
/// <code>
/// var application = JobApplicationEntity.Create(...);
/// var domainEvent = new JobApplicationCreatedEvent(application);
/// </code>
/// </example>
public record JobApplicationCreatedEvent(JobApplicationEntity JobApplicationEntity): IDomainEvent;