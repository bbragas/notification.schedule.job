namespace notification.scheduler.job.repository.mongo.Records;

public class WhatsAppRecord : INotificationRecord
{
    public Guid Id { get; }
    public string Message { get; set; }
    public string Subtitle { get; set; }
    public string PhoneNumber { get; set; }
    public int Type { get; set; }
    public NotificationStatusRecord Status { get; set; }
    public DateTime? ScheduledTo { get; set; }

}

