using Microsoft.Extensions.Logging;
using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.repository.mongo.DataSets.Selector;
using notification.scheduler.job.repository.mongo.Records;
using notification.scheduler.job.repository.mongo.Repositories;

namespace notification.scheduler.job.repository.mongo.DataSets.WhatsApp
{
    public class ScheduledWhatsAppReadableDataSet : IScheduledNotificationReadableDataSetSelector
    {
        private readonly IReadRepository _repository;
        private readonly ILogger<ScheduledWhatsAppReadableDataSet> _logger;

        public ScheduledWhatsAppReadableDataSet(IReadRepository repository, ILogger<ScheduledWhatsAppReadableDataSet> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<ScheduledNotification>> GetAllScheduledUntilNowAsync(
            CancellationToken cancellationToken)
        {
            var WhatsAppRecords =
                await _repository.GetByFilterAsync<WhatsAppRecord>(
                    record =>
                        record.Status == NotificationStatusRecord.Scheduled &&
                        record.ScheduledTo != null &&
                        record.ScheduledTo.Value <= DateTime.UtcNow,
                    cancellationToken);

            _logger.LogInformation("{Total} scheduled WhatsApp(s) found.", WhatsAppRecords.Count);

            return WhatsAppRecords.Select(record =>
                new ScheduledNotification(
                    new NotificationId(CalcWhatsAppType(record.Type), record.Id),
                    record.PhoneNumber,
                    null,
                    null,
                    null,
                    record.Subtitle,
                    record.Message,
                    record.ScheduledTo!.Value)).ToList();
        }

        public bool ApplyTo(NotificationId notificationId) => notificationId.Type == CalcWhatsAppType((int)notificationId.Type);

        private NotificationType CalcWhatsAppType(int type)
        {
            switch (type)
            {
                case (int)NotificationWhatsAppType.Link:
                    return NotificationType.WhatsAppLink;
                case (int)NotificationWhatsAppType.UrlMedia:
                    return NotificationType.WhatsAppMedia;
                case (int)NotificationWhatsAppType.Base64Media:
                    return NotificationType.WhatsAppMediaBase64;
                case (int)NotificationWhatsAppType.Text:
                    return NotificationType.WhatsAppText;
                default:
                    return NotificationType.WhatsAppText;

            }
        }
    }
}
