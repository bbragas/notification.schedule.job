namespace notification.scheduler.job.domain.Scheduled;

public record ScheduledNotification(
    NotificationId Id,
    string Contact,
    string? RecipientName,
    string? Campaign,
    string? SenderName,
    string? Subject,
    string Body,
    DateTime ScheduledTo);