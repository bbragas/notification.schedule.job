using System.Linq.Expressions;
using MongoDB.Driver;
using notification.scheduler.job.repository.mongo.Context;
using notification.scheduler.job.repository.mongo.Records;

namespace notification.scheduler.job.repository.mongo.Repositories;

public class WriteRepository : IWriteRepository
{
    private readonly IDbContext _dbContext;

    public WriteRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task UpdateFieldAsync<TRecord, TField>(Guid recordId, Expression<Func<TRecord, TField>> field, TField value, CancellationToken cancellationToken)
        where TRecord : IRecord
    {
        var filterByRecordId = Builders<TRecord>.Filter.Eq(x => x.Id, recordId);
        var updating = Builders<TRecord>.Update.Set(field, value);
        return _dbContext.GetCollection<TRecord>().UpdateOneAsync(filterByRecordId, updating, cancellationToken: cancellationToken);
    }
}