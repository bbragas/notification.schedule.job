using MediatR;
using notification.scheduler.job.domain.DataSets;
using notification.scheduler.job.domain.Scheduled;

namespace notification.scheduler.job.queries.Handlers.Notification;

public class GetAllScheduledNotificationsUntilNowQueryHandler : IRequestHandler<GetAllScheduledNotificationsUntilNowQuery, IReadOnlyCollection<ScheduledNotification>>
{
    private readonly IScheduledNotificationReadableDataSet _dataSet;

    public GetAllScheduledNotificationsUntilNowQueryHandler(IScheduledNotificationReadableDataSet dataSet)
    {
        _dataSet = dataSet;
    }

    public Task<IReadOnlyCollection<ScheduledNotification>> Handle(GetAllScheduledNotificationsUntilNowQuery request, CancellationToken cancellationToken) 
        => _dataSet.GetAllScheduledUntilNowAsync(cancellationToken);
}