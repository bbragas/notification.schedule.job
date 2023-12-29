using notification.scheduler.job.domain.DataSets;
using notification.scheduler.job.domain.Scheduled;

namespace notification.scheduler.job.repository.mongo.DataSets.Selector;

public interface IScheduledNotificationWriteableDataSetSelector : IScheduledNotificationWriteableDataSet
{
    bool ApplyTo(NotificationId notificationId);
}