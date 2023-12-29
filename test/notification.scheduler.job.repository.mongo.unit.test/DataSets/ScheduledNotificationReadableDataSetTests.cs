using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.repository.mongo.DataSets;
using notification.scheduler.job.repository.mongo.DataSets.Selector;
using NSubstitute;

namespace notification.scheduler.job.repository.mongo.unit.test.DataSets;

public class ScheduledNotificationReadableDataSetTests
{
    private readonly IScheduledNotificationReadableDataSetSelector _selector;
    private readonly ScheduledNotificationReadableDataSet _sut;
    
    public ScheduledNotificationReadableDataSetTests()
    {
        _selector = Substitute.For<IScheduledNotificationReadableDataSetSelector>();

        _sut = new ScheduledNotificationReadableDataSet(new List<IScheduledNotificationReadableDataSetSelector>
            { _selector, _selector });
    }
    
    
    [AutoFixtureData, Theory]
    public async Task When_get_all_scheduled_notifications_then_should_fetch_them_from_all_available_sources(List<ScheduledNotification> notifications)
    {
        _selector.GetAllScheduledUntilNowAsync(default).Returns(notifications);

        var results = await _sut.GetAllScheduledUntilNowAsync(default);
        
        Assert.Equal(notifications.Count * 2, results.Count);
    }
}