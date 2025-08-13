using MediatR;
using Microsoft.Extensions.Logging;

namespace Job.Domain.Jobs.Events.Handlers;

/// <summary>
/// Handles the <see cref="JobCreatedEvent"/> by logging when a new job is created.
/// </summary>
public class JobCreatedEventHandler(ILogger<JobCreatedEventHandler> logger)
    : INotificationHandler<JobCreatedEvent>
    
{
    /// <summary>
    /// Handles the <see cref="JobCreatedEvent"/> and logs the event name.
    /// </summary>
    /// <param name="notification">The job created domain event.</param>
    /// <param name="cancellationToken">A token for cancelling the operation.</param>
    /// <returns>A completed <see cref="Task"/> representing the operation.</returns>
    /// <example>
    /// <code>
    /// // Automatically triggered by MediatR when JobCreatedEvent is published
    /// await mediator.Publish(new JobCreatedEvent(...job));
    /// </code>
    /// </example>
    public Task Handle(JobCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Job created: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}