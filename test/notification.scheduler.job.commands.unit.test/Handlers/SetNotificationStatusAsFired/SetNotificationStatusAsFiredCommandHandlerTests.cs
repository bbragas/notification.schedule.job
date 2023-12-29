using AutoFixture.Xunit2;
using notification.scheduler.job.commands.Handlers.SetNotificationStatusAsFired;
using notification.scheduler.job.domain.DataSets;
using NSubstitute;

namespace notification.scheduler.job.commands.unit.test.Handlers.SetNotificationStatusAsFired;

public class SetNotificationStatusAsFiredCommandHandlerTests
{
    [AutoFixtureData, Theory]
    public async Task When_the_command_is_handled_then_update_the_notification_status_to_fired(
        [Frozen]IScheduledNotificationWriteableDataSet writeableDataSet, 
        SetNotificationStatusAsFiredCommandHandler sut, 
        SetNotificationStatusAsFiredCommand request)
    {
        await sut.Handle(request, default);

        await writeableDataSet.Received(1).SetStatusAsFiredAsync(request.NotificationId, default);
    }
}