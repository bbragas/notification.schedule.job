using MediatR;
using notification.scheduler.job.domain.Scheduled;

namespace notification.scheduler.job.queries.Handlers.Notification;

public record GetAllScheduledNotificationsUntilNowQuery : IRequest<IReadOnlyCollection<ScheduledNotification>>;