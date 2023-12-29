using Microsoft.Extensions.Logging;
using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.repository.mongo.DataSets.Selector;
using notification.scheduler.job.repository.mongo.Records;
using notification.scheduler.job.repository.mongo.Repositories;

namespace notification.scheduler.job.repository.mongo.DataSets.Email;

public class ScheduledEmailReadableDataSet : IScheduledNotificationReadableDataSetSelector
{
    private readonly IReadRepository _repository;
    private readonly ILogger<ScheduledEmailReadableDataSet> _logger;

    public ScheduledEmailReadableDataSet(IReadRepository repository, ILogger<ScheduledEmailReadableDataSet> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<ScheduledNotification>> GetAllScheduledUntilNowAsync(
        CancellationToken cancellationToken)
    {
        var emailRecords =
            await _repository.GetByFilterAsync<EmailRecord>(
                record => record.Status == NotificationStatusRecord.Scheduled &&
                          record.ScheduledTo.HasValue &&
                          record.ScheduledTo.Value <= DateTime.UtcNow,
                cancellationToken);

        _logger.LogInformation("{Total} scheduled email(s) found.", emailRecords.Count);
        
        return emailRecords.Select(record =>
            new ScheduledNotification(
                new NotificationId(NotificationType.Email, record.Id),
                record.RecipientEmail,
                record.RecipientName,
                record.Campaign,
                record.SenderName,
                record.Subject,
                record.Body,
                record.ScheduledTo!.Value)).ToList();
    }
    
    public bool ApplyTo(NotificationId notificationId) => notificationId.Type == NotificationType.Email;
}