using Microsoft.Extensions.Logging;
using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.repository.mongo.DataSets.Selector;
using notification.scheduler.job.repository.mongo.Records;
using notification.scheduler.job.repository.mongo.Repositories;

namespace notification.scheduler.job.repository.mongo.DataSets.Sms;

public class ScheduledSmsReadableDataSet : IScheduledNotificationReadableDataSetSelector
{
    private readonly IReadRepository _repository;
    private readonly ILogger<ScheduledSmsReadableDataSet> _logger;

    public ScheduledSmsReadableDataSet(IReadRepository repository, ILogger<ScheduledSmsReadableDataSet> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<ScheduledNotification>> GetAllScheduledUntilNowAsync(
        CancellationToken cancellationToken)
    {
        var smsRecords =
            await _repository.GetByFilterAsync<SmsRecord>(
                record =>
                    record.Status == NotificationStatusRecord.Scheduled &&
                    record.ScheduledTo != null &&
                    record.ScheduledTo.Value <= DateTime.UtcNow,
                cancellationToken);

        _logger.LogInformation("{Total} scheduled SMS(s) found.", smsRecords.Count);
        
        return smsRecords.Select(record =>
            new ScheduledNotification(
                new NotificationId(NotificationType.Sms, record.Id),
                record.PhoneNumber,
                null,
                record.Campaign,
                record.SenderName,
                null,
                record.Message,
                record.ScheduledTo!.Value)).ToList();
    }

    public bool ApplyTo(NotificationId notificationId) => notificationId.Type == NotificationType.Sms;
}