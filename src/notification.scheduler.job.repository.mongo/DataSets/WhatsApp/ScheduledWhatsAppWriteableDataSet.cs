using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.repository.mongo.DataSets.Selector;
using notification.scheduler.job.repository.mongo.Records;
using notification.scheduler.job.repository.mongo.Repositories;

namespace notification.scheduler.job.repository.mongo.DataSets.WhatsApp
{
    public class ScheduledWhatsAppWriteableDataSet : IScheduledNotificationWriteableDataSetSelector
    {
        private readonly IWriteRepository _repository;

        public ScheduledWhatsAppWriteableDataSet(IWriteRepository repository)
        {
            _repository = repository;
        }

        public bool ApplyTo(NotificationId notificationId)
            => notificationId.Type == CalcWhatsAppType((int)notificationId.Type);

        public Task SetStatusAsFiredAsync(NotificationId notificationId, CancellationToken cancellationToken) =>
            _repository.UpdateFieldAsync(
                notificationId.InternalId,
                (WhatsAppRecord record) => record.Status,
                NotificationStatusRecord.Fired,
                cancellationToken);


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
