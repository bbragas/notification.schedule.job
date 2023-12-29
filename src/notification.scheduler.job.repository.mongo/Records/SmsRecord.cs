namespace notification.scheduler.job.repository.mongo.Records;

public class SmsRecord : INotificationRecord
{
    public Guid Id { get; set; }
    public string Campaign { get; set; }
    public string Message { get; set; }
    public string? SenderName { get; set; }
    public string PhoneNumber { get; set; }
    public NotificationStatusRecord Status { get; set; }
    public DateTime? ScheduledTo { get; set; }
}