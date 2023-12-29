using AutoFixture.Xunit2;
using notification.scheduler.job.domain.DataSets;
using notification.scheduler.job.queries.Handlers.Notification;
using NSubstitute;

namespace notification.scheduler.job.queries.unit.test.Handlers.Notification;

public class GetAllScheduledNotificationsUntilNowQueryHandlerTests
{

    [AutoFixtureData, Theory]
    public async Task Given_there_are_multiple_scheduled_notifications_available_when_publish_all_then_should_publish_all(
        [Frozen]IScheduledNotificationReadableDataSet dataSet, 
        GetAllScheduledNotificationsUntilNowQueryHandler sut) 
    {
        await sut.Handle(new GetAllScheduledNotificationsUntilNowQuery(), default);

        await dataSet.Received(1).GetAllScheduledUntilNowAsync(default);
    }
}