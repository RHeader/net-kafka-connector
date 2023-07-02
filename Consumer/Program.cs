using Consumer.Handlers;
using Consumer.Model;
using Serilog;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKafkaConsumer<string, ReactiveInstance, ReactiveHandler>
(options =>
{
    options.Topic = "base_";
    options.BootstrapServers = "localhost:9092,localhost:9093,localhost:9094";
    options.GroupId = "base_group";
});

builder.Host.UseSerilog((context, options) =>
{
    options.WriteTo.Console();
});

var app = builder.Build();

app.Run();