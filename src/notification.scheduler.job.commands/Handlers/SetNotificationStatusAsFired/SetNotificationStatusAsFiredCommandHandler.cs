using MediatR;
using notification.scheduler.job.domain.DataSets;

namespace notification.scheduler.job.commands.Handlers.SetNotificationStatusAsFired;

public class SetNotificationStatusAsFiredCommandHandler : IRequestHandler<SetNotificationStatusAsFiredCommand, Unit>
{
    private readonly IScheduledNotificationWriteableDataSet _writeableDataSet;

    public SetNotificationStatusAsFiredCommandHandler(IScheduledNotificationWriteableDataSet writeableDataSet)
    {
        _writeableDataSet = writeableDataSet;
    }
    
    public async Task<Unit> Handle(SetNotificationStatusAsFiredCommand request, CancellationToken cancellationToken)
    {
        await _writeableDataSet.SetStatusAsFiredAsync(request.NotificationId, cancellationToken);
        return default;
    }
}