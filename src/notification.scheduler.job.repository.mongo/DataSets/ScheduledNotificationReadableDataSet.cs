using notification.scheduler.job.domain.DataSets;
using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.repository.mongo.DataSets.Selector;

namespace notification.scheduler.job.repository.mongo.DataSets;

public class ScheduledNotificationReadableDataSet : IScheduledNotificationReadableDataSet
{
    private readonly IEnumerable<IScheduledNotificationReadableDataSetSelector> _selectors;

    public ScheduledNotificationReadableDataSet(IEnumerable<IScheduledNotificationReadableDataSetSelector> selectors)
    {
        _selectors = selectors;
    }

    public async Task<IReadOnlyCollection<ScheduledNotification>> GetAllScheduledUntilNowAsync(
        CancellationToken cancellationToken)
    {
        var scheduledNotificationTask = _selectors.Select(x => x.GetAllScheduledUntilNowAsync(cancellationToken));
        var scheduledNotifications = await Task.WhenAll(scheduledNotificationTask);
        return scheduledNotifications.SelectMany(notification => notification).ToList();
    }
}