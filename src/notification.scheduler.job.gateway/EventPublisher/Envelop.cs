namespace notification.scheduler.job.gateway.EventPublisher;

public record Envelop(
    Guid Id,
    string SpecVersion,
    string Type,
    string Source,
    string Subject,
    string Time,
    string DataContentType,
    string Data);