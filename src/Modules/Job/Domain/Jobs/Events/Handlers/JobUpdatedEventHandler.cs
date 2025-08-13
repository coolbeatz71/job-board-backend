using MediatR;
using Microsoft.Extensions.Logging;

namespace Job.Domain.Jobs.Events.Handlers;

/// <summary>
/// Handles the <see cref="JobUpdatedEvent"/> by logging the event.
/// </summary>
/// <remarks>
/// Intended for future extension where a job change integration event will be published (e.g., to update application).
/// </remarks>
public class JobUpdatedEventHandler(ILogger<JobUpdatedEventHandler> logger)
    : INotificationHandler<JobUpdatedEvent>

{
    /// <summary>
    /// Handles the <see cref="JobUpdatedEvent"/> by logging information about the updated job.
    /// </summary>
    /// <param name="notification">The <see cref="JobUpdatedEvent"/> containing the updated job entity.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A completed <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <example>
    /// <code>
    /// // Triggered automatically by MediatR when the job entity is updated
    /// await mediator.Publish(new JobUpdatedEvent(...job));
    /// </code>
    /// </example>
    public Task Handle(JobUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Job updated: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}