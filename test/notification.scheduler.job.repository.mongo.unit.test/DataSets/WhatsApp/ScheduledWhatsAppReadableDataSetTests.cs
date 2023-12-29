using System.Linq.Expressions;
using AutoFixture.Xunit2;
using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.repository.mongo.DataSets.WhatsApp;
using notification.scheduler.job.repository.mongo.Records;
using notification.scheduler.job.repository.mongo.Repositories;
using NSubstitute;

namespace notification.scheduler.job.repository.mongo.unit.test.DataSets.WhatsApp;

public class ScheduledWhatsAppReadableDataSetTests
{
    [AutoFixtureData, Theory]
    public async Task Given_scheduled_WhatsApp_records_when_get_get_then_should_return_scheduled_notification_WhatsApp_type(
        [Frozen] IReadRepository repository,
        ScheduledWhatsAppReadableDataSet sut,
        WhatsAppRecord record)
    {
        repository.GetByFilterAsync(Arg.Any<Expression<Func<WhatsAppRecord, bool>>>(), default)
            .Returns(new List<WhatsAppRecord> { record });

        var results = await sut.GetAllScheduledUntilNowAsync(default);

        Assert.Collection(results, notification =>
        {
            Assert.Equal(CalcWhatsAppType(record.Type), notification.Id.Type);
            Assert.Equal(record.Id, notification.Id.InternalId);
            Assert.Equal(record.PhoneNumber, notification.Contact);
            Assert.Null(notification.RecipientName);
            Assert.Null(notification.SenderName);
            Assert.Equal(record.Message, notification.Body);
            Assert.Equal(record.ScheduledTo, notification.ScheduledTo);
        });
    }
    
    [AutoFixtureData, Theory]
    public async Task Given_scheduled_WhatsApp_records_when_get_get_then_should_query_the_scheduled_until_now_only(
        [Frozen] IReadRepository repository,
        ScheduledWhatsAppReadableDataSet sut,
        WhatsAppRecord recordNotScheduled,
        WhatsAppRecord recordScheduledToYesterday,
        WhatsAppRecord recordScheduledToTomorrow)

    {
        recordNotScheduled.Status = NotificationStatusRecord.Fired;
        recordScheduledToYesterday.Status = NotificationStatusRecord.Scheduled;
        recordScheduledToTomorrow.Status = NotificationStatusRecord.Scheduled;

        recordNotScheduled.ScheduledTo = null;
        recordScheduledToYesterday.ScheduledTo = DateTime.UtcNow.AddDays(-1);
        recordScheduledToTomorrow.ScheduledTo = DateTime.UtcNow.AddDays(1);

        var records = new List<WhatsAppRecord>
        {
            recordNotScheduled,
            recordScheduledToTomorrow,
            recordScheduledToYesterday
        };
        
        Expression<Func<WhatsAppRecord, bool>> filterUsed = null;
        repository.GetByFilterAsync(Arg.Do<Expression<Func<WhatsAppRecord, bool>>>(filter => filterUsed = filter), default)
            .Returns(new List<WhatsAppRecord> {recordScheduledToYesterday});

        await sut.GetAllScheduledUntilNowAsync(default);
        
        Assert.Collection(records.Where(filterUsed.Compile()), WhatsAppRecord => Assert.Equal(WhatsAppRecord, recordScheduledToYesterday));
    }
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