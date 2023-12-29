using System.Text.Json;

namespace notification.scheduler.job.gateway.Events.v1;

public record struct EmailEvent(
    Guid Id,
    string To,
    string Name,
    string Subject,
    string SenderName,
    string Html) : IEvent
{
    public string Version => "1.0";
    public string Source => "Notification.API";
    public string ContentType => "application/json";
    public string Type => "Notification.Api.Infrastructure.Messages.v1.SendEmailEvent";
    public string Description => "Email event";
    
    public string ToData(JsonSerializerOptions options)
    {
        var data = new
        {
            Id, 
            To,
            Name, 
            Subject, 
            SenderName, 
            Html
        };

        return JsonSerializer.Serialize(data, options);
    }
}