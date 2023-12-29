namespace notification.scheduler.job.repository.mongo.Records;

public interface INotificationRecord : IRecord
{
    DateTime? ScheduledTo { get; }
    NotificationStatusRecord Status { get; }
}