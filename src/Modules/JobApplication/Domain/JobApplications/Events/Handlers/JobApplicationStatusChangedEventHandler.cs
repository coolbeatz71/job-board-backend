using MediatR;
using Microsoft.Extensions.Logging;

namespace JobApplication.Domain.JobApplications.Events.Handlers;

/// <summary>
/// Handles the <see cref="JobApplicationStatusChangedEvent"/> by logging the event.
/// </summary>
/// <remarks>
/// Intended for future extension where a status change integration event will be published (e.g., to update users).
/// </remarks>
public class JobApplicationStatusChangedEventHandler(ILogger<JobApplicationStatusChangedEvent> logger):
    INotificationHandler<JobApplicationStatusChangedEvent>
{
    /// <summary>
    /// Handles the <see cref="JobApplicationStatusChangedEvent"/> and logs its occurrence.
    /// </summary>
    /// <param name="notification">The job application status changed domain event.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A completed <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <example>
    /// <code>
    /// // Triggered automatically by MediatR when the job application status is updated
    /// await mediator.Publish(new JobApplicationStatusChangedEvent(...jobApplication));
    /// </code>
    /// </example>
    public Task Handle(JobApplicationStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        // TODO: needs to publish job application status change integration event to update applicants
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}