using MongoDB.Bson.Serialization;
using notification.scheduler.job.repository.mongo.Records;

namespace notification.scheduler.job.repository.mongo.Mappers;

internal class SmsRecordCollectionMapper : ICollectionMapper
{
    public void Map() =>
        BsonClassMap.RegisterClassMap<SmsRecord>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });
}