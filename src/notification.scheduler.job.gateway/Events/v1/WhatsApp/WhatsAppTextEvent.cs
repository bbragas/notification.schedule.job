using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace notification.scheduler.job.gateway.Events.v1.WhatsApp
{
    public record struct WhatsAppTextEvent(
    Guid Id,
    string Campaign,
    string? SenderName,
    string To,
    string Text) : IEvent
    {
        public string Version => "1.0";
        public string Source => "Notification.API";
        public string ContentType => "application/json";
        public string Type => "Notification.Api.Infrastructure.Messages.v1.WhatsApp.SendTextWhatsAppEvent";
        public string Description => "WhatsApp link event";

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
}
