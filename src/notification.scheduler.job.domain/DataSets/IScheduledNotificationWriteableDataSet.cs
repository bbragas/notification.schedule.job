using notification.scheduler.job.domain.Scheduled;

namespace notification.scheduler.job.domain.DataSets;

public interface IScheduledNotificationWriteableDataSet
{
    Task SetStatusAsFiredAsync(NotificationId notificationId, CancellationToken cancellationToken);
}