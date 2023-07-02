using Producer.Model;
using Quartz;
using Shared;

namespace Producer.EventJobs;

[DisallowConcurrentExecution]
public class ReactiveInstanceCreatorJob : IJob
{
    private readonly IKafkaBus<string, ReactiveInstance> _bus;
    private readonly ILogger<ReactiveInstanceCreatorJob> _logger;

    private static double _baseIndex = 1;

    public ReactiveInstanceCreatorJob(IKafkaBus<string, ReactiveInstance> bus,
        ILogger<ReactiveInstanceCreatorJob> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _baseIndex *= double.E;
        
        for (long i = 0; i < _baseIndex; i++)
        {
            await _bus.PublishAsync(Guid.NewGuid().ToString(), new ReactiveInstance()
            {
               Id = Guid.NewGuid(),
               CreatorId = Guid.NewGuid(),
               CreatedAt = DateTime.UtcNow
            });
        }
        _logger.LogInformation("Job {job} Run Time: {time}",context.JobDetail.Key.Name, context.JobRunTime);
        _logger.LogInformation("Finished publish events, current index:{index}", _baseIndex);
    }
}