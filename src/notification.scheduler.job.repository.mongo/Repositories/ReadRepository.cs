using System.Linq.Expressions;
using MongoDB.Driver;
using notification.scheduler.job.repository.mongo.Context;

namespace notification.scheduler.job.repository.mongo.Repositories;

internal class ReadRepository : IReadRepository
{
    private readonly IDbContext _dbContext;

    public ReadRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<List<TRecord>> GetByFilterAsync<TRecord>(Expression<Func<TRecord, bool>> filter, CancellationToken cancellationToken) 
        => _dbContext.GetCollection<TRecord>().Find(filter).ToListAsync(cancellationToken);
}