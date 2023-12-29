namespace notification.scheduler.job.crosscutting.Tasks;

public static class TaskExtensions
{
    public static Task OnSuccess<TResult>(this Task task, Func<Task, TResult> continuationFunction)
    {
        return task.ContinueWith(continuationFunction, TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.AttachedToParent);
    }
}