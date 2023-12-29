using MediatR;
using notification.scheduler.job.domain.Scheduled;

namespace notification.scheduler.job.commands.Handlers.SetNotificationStatusAsFired;

public record SetNotificationStatusAsFiredCommand(NotificationId NotificationId) : IRequest<Unit>;