using Producer.EventJobs;
using Producer.Model;
using Quartz;
using Quartz.AspNetCore;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    // Just use the name of your job that you created in the Jobs folder.
    var jobKey = new JobKey("CreateEventManager");
    q.AddJob<ReactiveInstanceCreatorJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("event_trigger")
        .StartNow()
        .WithSimpleSchedule(x =>
        {
            x.WithIntervalInSeconds(5).RepeatForever();
        }));
});

builder.Services.AddQuartzServer(options =>
{
    options.WaitForJobsToComplete = true;
});

builder.Services.AddKafkaMessageBus();

builder.Services.AddKafkaProducer<string, ReactiveInstance>(options =>
{
    options.Topic = "base_";
    options.BootstrapServers = "localhost:9092,localhost:9093,localhost:9094";
});

var app = builder.Build();

app.Run();