using MongoDB.Bson.Serialization;
using notification.scheduler.job.repository.mongo.Records;

namespace notification.scheduler.job.repository.mongo.Mappers
{
    internal class WhatsAppRecordCollectionMapper : ICollectionMapper
    {
        public void Map() =>
            BsonClassMap.RegisterClassMap<WhatsAppRecord>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
    }
}
