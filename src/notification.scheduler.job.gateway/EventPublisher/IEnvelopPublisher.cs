namespace notification.scheduler.job.gateway.EventPublisher;

public interface IEnvelopPublisher
{
    Task PublishAsync(Envelop envelop, CancellationToken cancellationToken);
}