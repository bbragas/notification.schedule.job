using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using notification.scheduler.job.commands;
using notification.scheduler.job.gateway.sqs;
using notification.scheduler.job.queries;
using notification.scheduler.job.repository.mongo;
using Serilog;

namespace notification.scheduler.job;

public abstract class FunctionStartup
{
    protected readonly IServiceProvider ServiceProvider;

    public FunctionStartup()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .Build();
        
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        var services = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddLogging( builder => builder.AddSerilog(dispose: false))
            .AddCommands()
            .AddQueries()
            .AddGatewaySqs()
            .AddRepositoryMongo();

        ServiceProvider = services.BuildServiceProvider();
    }
}