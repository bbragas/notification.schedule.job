using Microsoft.Extensions.Options;
using MongoDB.Driver;
using notification.scheduler.job.repository.mongo.Mappers;
using notification.scheduler.job.repository.mongo.Settings;

namespace notification.scheduler.job.repository.mongo.Context;

public class DbContext : IDbContext
{
    private readonly Lazy<IMongoDatabase> _database;

    public DbContext(IOptions<MongoSettings> mongoSettings, IEnumerable<ICollectionMapper> collectionMappers)
    {
        _database = new Lazy<IMongoDatabase>(() =>
        {
            CreateMapper(collectionMappers);
            var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
            return mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        });
    }

    public IMongoCollection<TRecord> GetCollection<TRecord>()
    {
        var collectionName = typeof(TRecord).Name.Replace("Record", string.Empty);
        return _database.Value.GetCollection<TRecord>(collectionName);
    }
    
    private void CreateMapper(IEnumerable<ICollectionMapper> collectionMappers)
    {
        foreach (var collectionMapper in collectionMappers)
        {
            collectionMapper.Map();
        }
    }
}