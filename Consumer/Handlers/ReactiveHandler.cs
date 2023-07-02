using System.Text.Json;
using Consumer.Model;
using Shared.ConsumerActions;

namespace Consumer.Handlers;

public class ReactiveHandler : IConsumerHandler<string, ReactiveInstance>
{
    private readonly ILogger<ReactiveHandler> _logger;

    public ReactiveHandler(ILogger<ReactiveHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(string key, ReactiveInstance value)
    {
        _logger.LogInformation("Consumed instance : {instance} with {key}", JsonSerializer.Serialize(value), key);
    }
}