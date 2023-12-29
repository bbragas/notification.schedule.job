namespace notification.scheduler.job.domain.Scheduled;

public enum NotificationType
{
    Email = 1,
    Sms = 2,
    WhatsAppLink = 3,
    WhatsAppMediaBase64 = 4,
    WhatsAppMedia = 5,
    WhatsAppText = 6,
}