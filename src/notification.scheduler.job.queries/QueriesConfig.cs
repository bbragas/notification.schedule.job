using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace notification.scheduler.job.queries;

public static class QueriesConfig
{
    public static IServiceCollection AddQueries(this IServiceCollection services) =>
        services.AddMediatR(typeof(QueriesConfig).Assembly);
}