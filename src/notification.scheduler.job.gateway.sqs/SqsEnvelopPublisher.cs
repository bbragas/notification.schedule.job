using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using notification.scheduler.job.gateway.EventPublisher;
using notification.scheduler.job.gateway.sqs.Settings;

namespace notification.scheduler.job.gateway.sqs;

public class SqsEnvelopPublisher : IEnvelopPublisher
{
    private readonly IAmazonSQS _amazonSqs;
    private readonly SqsSettings _settings;
    private readonly ILogger<SqsEnvelopPublisher> _logger;

    public SqsEnvelopPublisher(IAmazonSQS amazonSqs, IOptions<SqsSettings> settings,
        ILogger<SqsEnvelopPublisher> logger)
    {
        _amazonSqs = amazonSqs;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task PublishAsync(Envelop envelop, CancellationToken cancellationToken)
    {
        var message = JsonSerializer.Serialize(envelop);
        var request = new SendMessageRequest(_settings.QueueUrl, message);

        var result = await _amazonSqs.SendMessageAsync(request, cancellationToken);

        var isSuccessResult = (int)result.HttpStatusCode >= 200 && (int)result.HttpStatusCode <= 299;
        if (!isSuccessResult)
        {
            _logger.LogError(
                "It was not possible to publish the envelop. Envelop={@Envelop}, QueueUrl={QueueUrl}, PublishingResult={@PublishingResult}",
                envelop,
                _settings.QueueUrl,
                result);
            throw new EnvelopPublishingException(envelop.Id, "It was not possible to publish the envelop.");
        }
    }
}