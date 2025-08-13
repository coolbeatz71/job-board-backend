using Core.Domain;
using JobApplication.Domain.JobApplications.Entities;

namespace JobApplication.Domain.JobApplications.Events;

/// <summary>
/// Represents a domain event that is raised when the status of a <see cref="JobApplicationEntity"/> changes.
/// </summary>
/// <param name="jobApplicationEntity">The job application entity whose status was updated.</param>
/// <remarks>
/// This event can be used to trigger updates in dependent systems, such as job or status history logs.
/// </remarks>
/// <example>
/// <code>
/// <![CDATA[
/// application.Update(..., Status: ApplicationStatus.Shortlisted);
/// var domainEvent = new JobApplicationStatusChangedEvent(application);
/// ]]>
/// </code>
/// </example>
public record JobApplicationStatusChangedEvent(JobApplicationEntity JobApplicationEntity): IDomainEvent;