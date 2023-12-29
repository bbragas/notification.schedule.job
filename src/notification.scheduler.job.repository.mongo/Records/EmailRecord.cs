namespace notification.scheduler.job.repository.mongo.Records;

public class EmailRecord : INotificationRecord
{
    public Guid Id { get; set; }
    public string Campaign { get; set; }
    public string SenderName { get; set; }
    public string SenderEmail { get; set; }
    public string RecipientName { get; set; }
    public string RecipientEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public DateTime? ScheduledTo { get; set; }
    public NotificationStatusRecord Status { get; set; }
}