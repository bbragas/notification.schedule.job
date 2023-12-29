namespace notification.scheduler.job.domain.Scheduled;

public record NotificationId(NotificationType Type, Guid InternalId);