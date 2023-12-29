using System.Net;
using System.Text.Json;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using notification.scheduler.job.gateway.EventPublisher;
using notification.scheduler.job.gateway.sqs.Settings;
using NSubstitute;

namespace notification.scheduler.job.gateway.sqs.unit.test;

public class SqsEnvelopPublisherTests
{
    private readonly Fixture _fixture = new(); 
        
    private readonly SqsEnvelopPublisher _sut;
    private readonly IAmazonSQS _amazonSqs;
    private readonly SqsSettings _settings;

    public SqsEnvelopPublisherTests()
    {
        _amazonSqs = Substitute.For<IAmazonSQS>();
        _settings = _fixture.Create<SqsSettings>();
        
        _sut = new SqsEnvelopPublisher(_amazonSqs, Options.Create(_settings), new NullLogger<SqsEnvelopPublisher>());
    }
    
    [AutoFixtureData, Theory]
    public async Task Given_an_envelop_message_when_publish_it_then_should_use_the_expected_queue_url_and_message_body(Envelop envelop)
    {
        SendMessageRequest? requestUsed = null;
        _amazonSqs.SendMessageAsync(Arg.Do<SendMessageRequest>(request => requestUsed = request), default).Returns(
            new SendMessageResponse
        {
            HttpStatusCode = HttpStatusCode.OK
        });
        
        await _sut.PublishAsync(envelop, default);
        
        Assert.Equal(_settings.QueueUrl, requestUsed!.QueueUrl);
        Assert.Equal(JsonSerializer.Serialize(envelop), requestUsed!.MessageBody);
    }

    [AutoFixtureData, Theory]
    public async Task Given_an_envelop_message_when_the_publishing_result_is_not_success_then_should_thrown_an_exception(Envelop envelop)
    {
        var random = new Random();
        _amazonSqs.SendMessageAsync(Arg.Any<SendMessageRequest>(), default).Returns(
            new SendMessageResponse
            {
                HttpStatusCode = (HttpStatusCode)random.Next(300, 600)
            });
        
        var action = () => _sut.PublishAsync(envelop, default);

        var exception = await Assert.ThrowsAsync<EnvelopPublishingException>(action);
        Assert.Equal($"EnvelopId={envelop.Id}, Details=It was not possible to publish the envelop.", exception.Message);
    }
    
}