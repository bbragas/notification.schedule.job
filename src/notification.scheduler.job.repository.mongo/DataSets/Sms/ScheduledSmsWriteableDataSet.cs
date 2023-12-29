using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.repository.mongo.DataSets.Selector;
using notification.scheduler.job.repository.mongo.Records;
using notification.scheduler.job.repository.mongo.Repositories;

namespace notification.scheduler.job.repository.mongo.DataSets.Sms;

public class ScheduledSmsWriteableDataSet : IScheduledNotificationWriteableDataSetSelector
{
    private readonly IWriteRepository _repository;

    public ScheduledSmsWriteableDataSet(IWriteRepository repository)
    {
        _repository = repository;
    }

    public bool ApplyTo(NotificationId notificationId)
        => notificationId.Type == NotificationType.Sms;

    public Task SetStatusAsFiredAsync(NotificationId notificationId, CancellationToken cancellationToken) =>
        _repository.UpdateFieldAsync(
            notificationId.InternalId, 
            (SmsRecord record) => record.Status, 
            NotificationStatusRecord.Fired,
            cancellationToken);
}