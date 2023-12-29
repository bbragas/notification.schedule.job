using Amazon.Lambda.TestUtilities;

namespace notification.scheduler.job.acceptance.test;

public class ExplorationTest
{
    [Fact(Skip = "exploration only")]
    public async Task Exploration_test_for_running_locally()
    {
        var anyRequest = new object();
        var context = new TestLambdaContext();
        
        var function = new Function();

        var response = await function.Handle(anyRequest, context);
    }
}