using MongoDB.Bson.Serialization;
using notification.scheduler.job.repository.mongo.Records;

namespace notification.scheduler.job.repository.mongo.Mappers;

internal class EmailRecordCollectionMapper : ICollectionMapper
{
    public void Map() =>
        BsonClassMap.RegisterClassMap<EmailRecord>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });
}