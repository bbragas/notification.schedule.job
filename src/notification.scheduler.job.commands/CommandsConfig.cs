using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace notification.scheduler.job.commands;

public static class CommandsConfig
{
    public static IServiceCollection AddCommands(this IServiceCollection services) =>
        services.AddMediatR(typeof(CommandsConfig).Assembly);
}