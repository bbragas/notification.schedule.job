using MediatR;

namespace notification.scheduler.job.commands.Handlers.PublishAllScheduledNotifications;

public record PublishAllScheduledNotificationsCommand : IRequest<Unit>;