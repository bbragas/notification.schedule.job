using System.Linq.Expressions;
using notification.scheduler.job.repository.mongo.Records;

namespace notification.scheduler.job.repository.mongo.Repositories;

public interface IWriteRepository
{
    Task UpdateFieldAsync<TRecord, TField>(Guid recordId, Expression<Func<TRecord, TField>> field, TField value, CancellationToken cancellationToken) 
        where TRecord : IRecord;
}