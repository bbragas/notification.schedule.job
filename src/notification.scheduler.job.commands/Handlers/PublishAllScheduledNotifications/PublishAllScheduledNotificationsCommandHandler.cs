using MediatR;
using Microsoft.Extensions.Logging;
using notification.scheduler.job.commands.Adapters;
using notification.scheduler.job.commands.Handlers.SetNotificationStatusAsFired;
using notification.scheduler.job.crosscutting.Tasks;
using notification.scheduler.job.gateway.EventPublisher;
using notification.scheduler.job.queries.Handlers.Notification;

namespace notification.scheduler.job.commands.Handlers.PublishAllScheduledNotifications;

public class
    PublishAllScheduledNotificationsCommandHandler : IRequestHandler<PublishAllScheduledNotificationsCommand, Unit>
{
    private readonly IMediator _mediator;
    private readonly IEnvelopPublisher _publisher;
    private readonly ILogger<PublishAllScheduledNotificationsCommandHandler> _logger;

    public PublishAllScheduledNotificationsCommandHandler(IMediator mediator, IEnvelopPublisher publisher,
        ILogger<PublishAllScheduledNotificationsCommandHandler> logger)
    {
        _mediator = mediator;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<Unit> Handle(PublishAllScheduledNotificationsCommand request, CancellationToken cancellationToken)
    {
        var query = new GetAllScheduledNotificationsUntilNowQuery();
        var scheduledNotifications = await _mediator.Send(query, cancellationToken);
        
        _logger.LogInformation("{Total} scheduled notification(s) found to be published.", scheduledNotifications.Count);

        var publishingTasks = scheduledNotifications.Select(
            notification =>
                 _publisher.PublishAsync(notification.ToEnvelop(), cancellationToken)
                    .OnSuccess(_ => _mediator.Send(new SetNotificationStatusAsFiredCommand(notification.Id), cancellationToken))).ToList();

        await WrapPublishingTasks(publishingTasks);

        return default;
    }

    private async Task WrapPublishingTasks(IReadOnlyCollection<Task> publishingTasks)
    {
        try
        {
            await Task.WhenAll(publishingTasks);
        }
        catch (Exception _)
        {
            var innerExceptions = publishingTasks.Where(task => task.Exception != null)
                                                         .SelectMany(task => task.Exception!.InnerExceptions);

            foreach (var exception in innerExceptions)
            {
                _logger.LogError(exception, "A error was happening when a scheduled notification was publishing.");
            }
        }
    }
}