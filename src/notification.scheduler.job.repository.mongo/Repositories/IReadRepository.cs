using System.Linq.Expressions;

namespace notification.scheduler.job.repository.mongo.Repositories;

public interface IReadRepository
{
    Task<List<TRecord>> GetByFilterAsync<TRecord>(Expression<Func<TRecord, bool>> filter, CancellationToken cancellationToken);
}