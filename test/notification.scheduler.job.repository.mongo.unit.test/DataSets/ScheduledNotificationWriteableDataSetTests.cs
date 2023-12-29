using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.repository.mongo.DataSets;
using notification.scheduler.job.repository.mongo.DataSets.Selector;
using NSubstitute;

namespace notification.scheduler.job.repository.mongo.unit.test.DataSets;

public class ScheduledNotificationWriteableDataSetTests
{
    private readonly IScheduledNotificationWriteableDataSetSelector _selector1;
    private readonly IScheduledNotificationWriteableDataSetSelector _selector2;
    private readonly ScheduledNotificationWriteableDataSet _sut;
    
    public ScheduledNotificationWriteableDataSetTests()
    {
        _selector1 = Substitute.For<IScheduledNotificationWriteableDataSetSelector>();
        _selector2 = Substitute.For<IScheduledNotificationWriteableDataSetSelector>();

        _sut = new ScheduledNotificationWriteableDataSet(new List<IScheduledNotificationWriteableDataSetSelector>
            { _selector1, _selector2 });
    }
    
    [AutoFixtureData, Theory]
    public async Task Given_the_there_is_a_selector_applicable_when_update_the_status_then_should_use_the_applicable_selector(NotificationId notificationId)
    {
        _selector1.ApplyTo(notificationId).Returns(true);

        await _sut.SetStatusAsFiredAsync(notificationId, default);

        await _selector1.Received(1).SetStatusAsFiredAsync(notificationId, default);
        await _selector2.DidNotReceive().SetStatusAsFiredAsync(notificationId, default);
    }
    
    [AutoFixtureData, Theory]
    public async Task Given_the_there_are_no_selectors_applicable_when_update_the_status_then_an_exceptions_is_thrown(NotificationId notificationId)
    {
        Func<Task> action = () => _sut.SetStatusAsFiredAsync(notificationId, default);

        var exception = await Assert.ThrowsAsync<ApplicationException>(action);
        Assert.Equal($"No data set were found for the {notificationId.Type} notification type.", exception.Message);
    }
}