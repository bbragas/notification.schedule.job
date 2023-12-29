using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using notification.scheduler.job.gateway.EventPublisher;
using notification.scheduler.job.gateway.sqs.Settings;

namespace notification.scheduler.job.gateway.sqs;

public static class GatewaySqsConfig
{
    public static IServiceCollection AddGatewaySqs(this IServiceCollection services)
    {
        services
            .AddAWSService<IAmazonSQS>()
            .AddScoped<IEnvelopPublisher, SqsEnvelopPublisher>()
            .AddOptions<SqsSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(nameof(SqsSettings)).Bind(settings);
            });

        return services;
    }
}