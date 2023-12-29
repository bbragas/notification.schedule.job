using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using notification.scheduler.job.domain.DataSets;
using notification.scheduler.job.repository.mongo.Context;
using notification.scheduler.job.repository.mongo.DataSets;
using notification.scheduler.job.repository.mongo.DataSets.Email;
using notification.scheduler.job.repository.mongo.DataSets.Selector;
using notification.scheduler.job.repository.mongo.DataSets.Sms;
using notification.scheduler.job.repository.mongo.Mappers;
using notification.scheduler.job.repository.mongo.Repositories;
using notification.scheduler.job.repository.mongo.Settings;

namespace notification.scheduler.job.repository.mongo;

public static class RepositoryMongoConfig
{
    public static IServiceCollection AddRepositoryMongo(this IServiceCollection services)
    {
         services
            .AddCollectionMapper()
            .AddScoped<IDbContext, DbContext>()
            .AddScoped<IScheduledNotificationReadableDataSet, ScheduledNotificationReadableDataSet>()
            .AddScoped<IWriteRepository, WriteRepository>()
            .AddScoped<IReadRepository, ReadRepository>()
            .AddScoped<IScheduledNotificationReadableDataSetSelector, ScheduledEmailReadableDataSet>()
            .AddScoped<IScheduledNotificationReadableDataSetSelector, ScheduledSmsReadableDataSet>()
            .AddScoped<IScheduledNotificationWriteableDataSetSelector, ScheduledEmailWriteableDataSet>()
            .AddScoped<IScheduledNotificationWriteableDataSetSelector, ScheduledSmsWriteableDataSet>()
            .AddScoped<IScheduledNotificationReadableDataSet, ScheduledNotificationReadableDataSet>()
            .AddScoped<IScheduledNotificationWriteableDataSet, ScheduledNotificationWriteableDataSet>()
            .AddOptions<MongoSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(nameof(MongoSettings)).Bind(settings);
            });

         return services;
    }

    private static IServiceCollection AddCollectionMapper(this IServiceCollection services)
    {
        var collectionMapperType = typeof(ICollectionMapper);
        var types = typeof(ICollectionMapper).Assembly.GetTypes();

        foreach (var item in types)
        {
            if (!item.IsInterface && collectionMapperType.IsAssignableFrom(item))
            {
                services.AddSingleton(collectionMapperType, item);
            }
        }
        return services;
    }
}