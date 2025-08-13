using MediatR;
using Microsoft.Extensions.Logging;

namespace JobApplication.Domain.JobApplications.Events.Handlers;

/// <summary>
/// Handles the <see cref="JobApplicationCreatedEvent"/> by logging when a new job application is created.
/// </summary>
public class JobApplicationCreatedEventHandler(ILogger<JobApplicationCreatedEvent> logger):
    INotificationHandler<JobApplicationCreatedEvent>
{
    /// <summary>
    /// Handles the <see cref="JobApplicationCreatedEvent"/> and logs the event name.
    /// </summary>
    /// <param name="notification">The job application created domain event.</param>
    /// <param name="cancellationToken">A token for cancelling the operation.</param>
    /// <returns>A completed <see cref="Task"/> representing the operation.</returns>
    /// <example>
    /// <code>
    /// // Automatically triggered by MediatR when JobApplicationCreatedEvent is published
    /// await mediator.Publish(new JobApplicationCreatedEvent(...jobApplication));
    /// </code>
    /// </example>
    public Task Handle(JobApplicationCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Job Application created: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}