using MongoDB.Driver;

namespace notification.scheduler.job.repository.mongo.Context;

public interface IDbContext
{
    IMongoCollection<TRecord> GetCollection<TRecord>();

}