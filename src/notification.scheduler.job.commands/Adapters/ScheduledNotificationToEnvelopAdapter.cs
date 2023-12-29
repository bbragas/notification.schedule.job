using System.Text.Json;
using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.gateway.EventPublisher;
using notification.scheduler.job.gateway.Events.v1;
using notification.scheduler.job.gateway.Events.v1.WhatsApp;

namespace notification.scheduler.job.commands.Adapters;

public static class ScheduledNotificationToEnvelopAdapter
{
    private static readonly JsonSerializerOptions Options = new ()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public static Envelop ToEnvelop(this ScheduledNotification scheduledNotification)
    {
        var @event = ToEvent(scheduledNotification);

        return new Envelop(
            @event.Id,
            @event.Version,
            @event.Type,
            @event.Source,
            @event.Description,
            DateTime.UtcNow.ToString(),
            @event.ContentType,
            @event.ToData(Options));
    }

    private static IEvent ToEvent(ScheduledNotification scheduledNotification)
    {
        switch (scheduledNotification.Id.Type)
        {
            case NotificationType.Sms:
                return new SmsEvent(
                    scheduledNotification.Id.InternalId,
                    scheduledNotification.Campaign,
                    scheduledNotification.SenderName,
                    scheduledNotification.Contact,
                    scheduledNotification.Body);

            case NotificationType.WhatsAppLink:
                return new WhatsAppLinkEvent(
                    scheduledNotification.Id.InternalId,
                    scheduledNotification.Campaign,
                    scheduledNotification.SenderName,
                    scheduledNotification.Contact,
                    scheduledNotification.Body);

            case NotificationType.WhatsAppMedia:
                return new WhatsAppMediaEvent(
                    scheduledNotification.Id.InternalId,
                    scheduledNotification.Campaign,
                    scheduledNotification.SenderName,
                    scheduledNotification.Contact,
                    scheduledNotification.Body);

            case NotificationType.WhatsAppMediaBase64:
                return new WhatsAppMediaBase64Event(
                    scheduledNotification.Id.InternalId,
                    scheduledNotification.Campaign,
                    scheduledNotification.SenderName,
                    scheduledNotification.Contact,
                    scheduledNotification.Body);

            case NotificationType.WhatsAppText:
                return new WhatsAppTextEvent(
                    scheduledNotification.Id.InternalId,
                    scheduledNotification.Campaign,
                    scheduledNotification.SenderName,
                    scheduledNotification.Contact,
                    scheduledNotification.Body);

            default:
                return new EmailEvent(
                    scheduledNotification.Id.InternalId,
                    scheduledNotification.Contact,
                    scheduledNotification.RecipientName,
                    scheduledNotification.Subject,
                    scheduledNotification.SenderName,
                    scheduledNotification.Body);
        }
    }
}