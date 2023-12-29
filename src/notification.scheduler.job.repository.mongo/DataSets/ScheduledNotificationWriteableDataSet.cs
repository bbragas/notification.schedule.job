using notification.scheduler.job.domain.DataSets;
using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.repository.mongo.DataSets.Selector;

namespace notification.scheduler.job.repository.mongo.DataSets;

public class ScheduledNotificationWriteableDataSet : IScheduledNotificationWriteableDataSet
{
    private readonly IEnumerable<IScheduledNotificationWriteableDataSetSelector> _selectors;

    public ScheduledNotificationWriteableDataSet(IEnumerable<IScheduledNotificationWriteableDataSetSelector> selectors)
    {
        _selectors = selectors;
    }

    public Task SetStatusAsFiredAsync(NotificationId notificationId, CancellationToken cancellationToken)
    {
        var dataSet = _selectors.FirstOrDefault(data => data.ApplyTo(notificationId));

        if (dataSet == null)
        {
            throw new ApplicationException($"No data set were found for the {notificationId.Type} notification type.");
        }
        
        return dataSet.SetStatusAsFiredAsync(notificationId, cancellationToken);
    }
}