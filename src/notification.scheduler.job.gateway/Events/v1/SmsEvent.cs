using System.Text.Json;

namespace notification.scheduler.job.gateway.Events.v1;

public record struct SmsEvent(
    Guid Id,
    string Campaign,
    string? SenderName,
    string To,
    string Text) : IEvent
{
    public string Version => "1.0";
    public string Source => "Notification.API";
    public string ContentType => "application/json";
    public string Type => "Notification.Api.Infrastructure.Messages.v1.SendSmsEvent";
    public string Description => "Sms event";
    
    public string ToData(JsonSerializerOptions options)
    {
        var data = new
        {
            Id, 
            Campaign,
            SenderName, 
            To, 
            Text 
        };

        return JsonSerializer.Serialize(data, options);
    }
}