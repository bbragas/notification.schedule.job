using System.Text.Json;
using AutoFixture;
using notification.scheduler.job.commands.Adapters;
using notification.scheduler.job.domain.Scheduled;

namespace notification.scheduler.job.commands.unit.test.Adapters;

public class ScheduledNotificationToEnvelopAdapterTests
{
    private readonly Fixture _fixture = new ();
    private readonly JsonSerializerOptions _options = new ()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    [AutoFixtureData, Theory]
    public void Given_a_scheduled_email_should_adapt_to_envelop(Guid internalId)
    {
        var scheduledEmail = _fixture.Build<ScheduledNotification>()
            .With(x => x.Id, new NotificationId(NotificationType.Email, internalId))
            .Create();

        var data = new
        {
            Id = scheduledEmail.Id.InternalId,
            To = scheduledEmail.Contact,
            Name = scheduledEmail.RecipientName,
            Subject = scheduledEmail.Subject,
            SenderName = scheduledEmail.SenderName,
            Html = scheduledEmail.Body
        };

        var expectedData = JsonSerializer.Serialize(data, _options);
        
        var result = scheduledEmail.ToEnvelop();
        
        Assert.Equal(internalId, result.Id);
        Assert.Equal("1.0", result.SpecVersion);
        Assert.Equal("Notification.Api.Infrastructure.Messages.v1.SendEmailEvent", result.Type);
        Assert.Equal("Notification.API", result.Source);
        Assert.Equal("application/json", result.DataContentType);
        Assert.Equal("Email event", result.Subject);
        Assert.NotEmpty(result.Time);
        Assert.Equal(expectedData, result.Data);
    }
    
    [AutoFixtureData, Theory]
    public void Given_a_scheduled_sms_should_adapt_to_envelop(Guid internalId)
    {
        var scheduledEmail = _fixture.Build<ScheduledNotification>()
            .With(x => x.Id, new NotificationId(NotificationType.Sms, internalId))
            .Create();

        var data = new
        {
            Id = scheduledEmail.Id.InternalId,
            Campaign = scheduledEmail.Campaign,
            SenderName = scheduledEmail.SenderName,
            To = scheduledEmail.Contact,
            Text = scheduledEmail.Body
        };

        var expectedData = JsonSerializer.Serialize(data, _options);
        
        var result = scheduledEmail.ToEnvelop();
        
        Assert.Equal(internalId, result.Id);
        Assert.Equal("1.0", result.SpecVersion);
        Assert.Equal("Notification.Api.Infrastructure.Messages.v1.SendSmsEvent", result.Type);
        Assert.Equal("Notification.API", result.Source);
        Assert.Equal("application/json", result.DataContentType);
        Assert.Equal("Sms event", result.Subject);
        Assert.NotEmpty(result.Time);
        Assert.Equal(expectedData, result.Data);
    }
    
}