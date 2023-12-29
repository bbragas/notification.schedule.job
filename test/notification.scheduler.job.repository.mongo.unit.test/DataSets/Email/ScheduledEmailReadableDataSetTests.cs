using System.Linq.Expressions;
using AutoFixture.Xunit2;
using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.repository.mongo.DataSets.Email;
using notification.scheduler.job.repository.mongo.Records;
using notification.scheduler.job.repository.mongo.Repositories;
using NSubstitute;

namespace notification.scheduler.job.repository.mongo.unit.test.DataSets.Email;

public class ScheduledEmailReadableDataSetTests
{
    [AutoFixtureData, Theory]
    public async Task Given_scheduled_email_records_when_get_get_then_should_return_scheduled_notification_email_type(
        [Frozen] IReadRepository repository,
        ScheduledEmailReadableDataSet sut,
        EmailRecord record)
    {
        repository.GetByFilterAsync(Arg.Any<Expression<Func<EmailRecord, bool>>>(), default)
            .Returns(new List<EmailRecord> { record });

        var results = await sut.GetAllScheduledUntilNowAsync(default);

        Assert.Collection(results, notification =>
        {
            Assert.Equal(NotificationType.Email, notification.Id.Type);
            Assert.Equal(record.Id, notification.Id.InternalId);
            Assert.Equal(record.RecipientEmail, notification.Contact);
            Assert.Equal(record.RecipientName, notification.RecipientName);
            Assert.Equal(record.Campaign, notification.Campaign);
            Assert.Equal(record.SenderName, notification.SenderName);
            Assert.Equal(record.Subject, notification.Subject);
            Assert.Equal(record.Body, notification.Body);
            Assert.Equal(record.ScheduledTo, notification.ScheduledTo);
        });
    }
    
    [AutoFixtureData, Theory]
    public async Task Given_scheduled_email_records_when_get_get_then_should_query_the_scheduled_until_now_only(
        [Frozen] IReadRepository repository,
        ScheduledEmailReadableDataSet sut,
        EmailRecord recordNotScheduled,
        EmailRecord recordScheduledToYesterday,
        EmailRecord recordScheduledToTomorrow)

    {
        recordNotScheduled.Status = NotificationStatusRecord.Fired;
        recordScheduledToYesterday.Status = NotificationStatusRecord.Scheduled;
        recordScheduledToTomorrow.Status = NotificationStatusRecord.Scheduled;
        
        recordNotScheduled.ScheduledTo = null;
        recordScheduledToYesterday.ScheduledTo = DateTime.UtcNow.AddDays(-1);
        recordScheduledToTomorrow.ScheduledTo = DateTime.UtcNow.AddDays(1);

        var records = new List<EmailRecord>
        {
            recordNotScheduled,
            recordScheduledToTomorrow,
            recordScheduledToYesterday
        };
        
        Expression<Func<EmailRecord, bool>> filterUsed = null;
        repository.GetByFilterAsync(Arg.Do<Expression<Func<EmailRecord, bool>>>(filter => filterUsed = filter), default)
            .Returns(new List<EmailRecord> {recordScheduledToYesterday});

        await sut.GetAllScheduledUntilNowAsync(default);
        
        Assert.Collection(records.Where(filterUsed.Compile()), emailRecord => Assert.Equal(emailRecord, recordScheduledToYesterday));
    }
}