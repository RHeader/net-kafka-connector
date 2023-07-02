using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Shared.Deserializers;

namespace Shared.ConsumerActions;

public class ConsumerBackgroundInstance<TKey, TData> : BackgroundService
{
    private readonly ConsumerConfiguration<TKey, TData> _config;
    private IConsumerHandler<TKey, TData> _handler;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ConsumerBackgroundInstance(IOptions<ConsumerConfiguration<TKey, TData>> config,
        IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _config = config.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            _handler = scope.ServiceProvider.GetRequiredService<IConsumerHandler<TKey, TData>>();

            var builder = new ConsumerBuilder<TKey, TData>(_config).SetValueDeserializer(new JsonKafkaDeserializer<TData>());

            using (IConsumer<TKey, TData> consumer = builder.Build())
            {
                consumer.Subscribe(_config.Topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(TimeSpan.FromMilliseconds(1000));

                    if (result != null)
                    {
                        await _handler.HandleAsync(result.Message.Key, result.Message.Value);

                        consumer.Commit(result);

                        consumer.StoreOffset(result);
                    }
                }
            }
        }
    }
}