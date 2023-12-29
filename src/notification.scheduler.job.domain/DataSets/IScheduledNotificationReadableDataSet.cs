using notification.scheduler.job.domain.Scheduled;

namespace notification.scheduler.job.domain.DataSets;

public interface IScheduledNotificationReadableDataSet
{
    Task<IReadOnlyCollection<ScheduledNotification>> GetAllScheduledUntilNowAsync(CancellationToken cancellationToken);
}