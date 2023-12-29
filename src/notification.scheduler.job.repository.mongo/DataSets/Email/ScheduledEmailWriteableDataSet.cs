using notification.scheduler.job.domain.DataSets;
using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.repository.mongo.DataSets.Selector;
using notification.scheduler.job.repository.mongo.Records;
using notification.scheduler.job.repository.mongo.Repositories;

namespace notification.scheduler.job.repository.mongo.DataSets.Email;

public class ScheduledEmailWriteableDataSet : IScheduledNotificationWriteableDataSetSelector
{
    private readonly IWriteRepository _repository;

    public ScheduledEmailWriteableDataSet(IWriteRepository repository)
    {
        _repository = repository;
    }

    public bool ApplyTo(NotificationId notificationId)
        => notificationId.Type == NotificationType.Email;

    public Task SetStatusAsFiredAsync(NotificationId notificationId, CancellationToken cancellationToken) =>
        _repository.UpdateFieldAsync(
            notificationId.InternalId, 
            (EmailRecord record) => record.Status, 
            NotificationStatusRecord.Fired,
            cancellationToken);
}