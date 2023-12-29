using System.Linq.Expressions;
using AutoFixture.Xunit2;
using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.repository.mongo.DataSets.Sms;
using notification.scheduler.job.repository.mongo.Records;
using notification.scheduler.job.repository.mongo.Repositories;
using NSubstitute;

namespace notification.scheduler.job.repository.mongo.unit.test.DataSets.Sms;

public class ScheduledSmsReadableDataSetTests
{
    [AutoFixtureData, Theory]
    public async Task Given_scheduled_sms_records_when_get_get_then_should_return_scheduled_notification_sms_type(
        [Frozen] IReadRepository repository,
        ScheduledSmsReadableDataSet sut,
        SmsRecord record)
    {
        repository.GetByFilterAsync(Arg.Any<Expression<Func<SmsRecord, bool>>>(), default)
            .Returns(new List<SmsRecord> { record });

        var results = await sut.GetAllScheduledUntilNowAsync(default);

        Assert.Collection(results, notification =>
        {
            Assert.Equal(NotificationType.Sms, notification.Id.Type);
            Assert.Equal(record.Id, notification.Id.InternalId);
            Assert.Equal(record.PhoneNumber, notification.Contact);
            Assert.Null(notification.RecipientName);
            Assert.Equal(record.Campaign, notification.Campaign);
            Assert.Equal(record.SenderName, notification.SenderName);
            Assert.Null(notification.Subject);
            Assert.Equal(record.Message, notification.Body);
            Assert.Equal(record.ScheduledTo, notification.ScheduledTo);
        });
    }
    
    [AutoFixtureData, Theory]
    public async Task Given_scheduled_sms_records_when_get_get_then_should_query_the_scheduled_until_now_only(
        [Frozen] IReadRepository repository,
        ScheduledSmsReadableDataSet sut,
        SmsRecord recordNotScheduled,
        SmsRecord recordScheduledToYesterday,
        SmsRecord recordScheduledToTomorrow)

    {
        recordNotScheduled.Status = NotificationStatusRecord.Fired;
        recordScheduledToYesterday.Status = NotificationStatusRecord.Scheduled;
        recordScheduledToTomorrow.Status = NotificationStatusRecord.Scheduled;
        
        recordNotScheduled.ScheduledTo = null;
        recordScheduledToYesterday.ScheduledTo = DateTime.UtcNow.AddDays(-1);
        recordScheduledToTomorrow.ScheduledTo = DateTime.UtcNow.AddDays(1);

        var records = new List<SmsRecord>
        {
            recordNotScheduled,
            recordScheduledToTomorrow,
            recordScheduledToYesterday
        };
        
        Expression<Func<SmsRecord, bool>> filterUsed = null;
        repository.GetByFilterAsync(Arg.Do<Expression<Func<SmsRecord, bool>>>(filter => filterUsed = filter), default)
            .Returns(new List<SmsRecord> {recordScheduledToYesterday});

        await sut.GetAllScheduledUntilNowAsync(default);
        
        Assert.Collection(records.Where(filterUsed.Compile()), smsRecord => Assert.Equal(smsRecord, recordScheduledToYesterday));
    }
}