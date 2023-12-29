using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using notification.scheduler.job.commands.Handlers.PublishAllScheduledNotifications;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
namespace notification.scheduler.job;

public class Function : FunctionStartup
{
    private readonly IMediator _mediator;

    public Function()
    {
        _mediator = ServiceProvider.GetRequiredService<IMediator>();
    }

    public async Task<APIGatewayProxyResponse> Handle(object _, ILambdaContext context)
    {
        context.Logger.LogInformation("notification.scheduler.job started.");

        var command = new PublishAllScheduledNotificationsCommand();
        await _mediator.Send(command, default);
        
        context.Logger.LogInformation("notification.scheduler.job ended.");
        
        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}}
        };
    }
}