using System.Text.Json;

namespace notification.scheduler.job.gateway.Events.v1;

public interface IEvent
{
    Guid Id { get; }
    string Version { get; }
    string Source { get; }
    string ContentType { get; }
    string Type { get; }
    string Description { get; }
    string ToData(JsonSerializerOptions options);
}