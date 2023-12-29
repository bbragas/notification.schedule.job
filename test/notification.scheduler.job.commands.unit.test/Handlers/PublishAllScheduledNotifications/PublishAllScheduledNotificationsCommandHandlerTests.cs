using AutoFixture.Xunit2;
using MediatR;
using notification.scheduler.job.commands.Handlers.PublishAllScheduledNotifications;
using notification.scheduler.job.commands.Handlers.SetNotificationStatusAsFired;
using notification.scheduler.job.domain.Scheduled;
using notification.scheduler.job.gateway.EventPublisher;
using notification.scheduler.job.queries.Handlers.Notification;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace notification.scheduler.job.commands.unit.test.Handlers.PublishAllScheduledNotifications;

public class PublishAllScheduledNotificationsCommandHandlerTests
{
    [AutoFixtureData, Theory]
    public async Task Given_there_are_multiple_scheduled_notifications_available_when_the_command_is_handled_then_should_publish_and_update_status_of_all_notifications(
        [Frozen]IMediator mediator, 
        [Frozen]IEnvelopPublisher publisher, 
        PublishAllScheduledNotificationsCommandHandler sut, 
        List<ScheduledNotification> notifications)
    {
        mediator.Send(Arg.Any<GetAllScheduledNotificationsUntilNowQuery>(), default)
            .Returns(notifications);

        await sut.Handle(new PublishAllScheduledNotificationsCommand(), default);

        var verifyPublishingTasks = notifications.Select(notification => VerifySuccessfulPublishing(mediator, publisher, notification));
        await Task.WhenAll(verifyPublishingTasks);
    }
   
    [AutoFixtureData, Theory]
    public async Task Given_there_are_poison_and_health_scheduled_notifications_available_when_the_command_is_handled_then_should_update_status_of_the_health_ones_only(
        [Frozen]IMediator mediator, 
        [Frozen]IEnvelopPublisher publisher, 
        PublishAllScheduledNotificationsCommandHandler sut, 
        List<ScheduledNotification> healthNotifications,
        List<ScheduledNotification> poisonNotifications)
    {
        var notifications = poisonNotifications
            .Concat(healthNotifications)
            .OrderBy(x => Guid.NewGuid())
            .ToList();
        
        mediator.Send(Arg.Any<GetAllScheduledNotificationsUntilNowQuery>(), default)
            .Returns(notifications);

        publisher.PublishAsync(
                Arg.Is<Envelop>(env => NotificationHasId(env.Id, poisonNotifications)), 
                default)
            .ThrowsAsync(new Exception("Kabum!!"));


        await sut.Handle(new PublishAllScheduledNotificationsCommand(), default);
        
        var verifySuccessfulPublishingTasks = healthNotifications.Select(notification => VerifySuccessfulPublishing(mediator, publisher, notification));
        await Task.WhenAll(verifySuccessfulPublishingTasks);
        
        var verifyUnsuccessfulPublishingTasks = poisonNotifications.Select(notification => VerifyUnsuccessfulPublishing(mediator, publisher, notification));
        await Task.WhenAll(verifyUnsuccessfulPublishingTasks);
        
    }
    private async Task VerifySuccessfulPublishing(IMediator mediator, IEnvelopPublisher publisher, ScheduledNotification notification)
    {
        await publisher.Received(1).PublishAsync(
            Arg.Is<Envelop>(x => x.Id == notification.Id.InternalId), 
            default);

        await mediator.Received(1)
            .Send(Arg.Is<SetNotificationStatusAsFiredCommand>(x => x.NotificationId == notification.Id));
    }
    
    private async Task VerifyUnsuccessfulPublishing(IMediator mediator, IEnvelopPublisher publisher, ScheduledNotification notification)
    {
        await publisher.Received(1).PublishAsync(
            Arg.Is<Envelop>(x => x.Id == notification.Id.InternalId), 
            default);

        await mediator.DidNotReceive()
            .Send(Arg.Is<SetNotificationStatusAsFiredCommand>(x => x.NotificationId == notification.Id));
    }
    
    private bool NotificationHasId(Guid id, List<ScheduledNotification> notifications) => notifications.Any(notification => notification.Id.InternalId == id);
}