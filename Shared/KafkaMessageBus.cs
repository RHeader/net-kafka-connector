using Shared.ProducerActions;

namespace Shared;

public class KafkaMessageBus<TKey, TData> : IKafkaBus<TKey, TData>
{
    private readonly BaseProducer<TKey, TData> _producer;
    public KafkaMessageBus(BaseProducer<TKey,TData> producer)
    {
        _producer = producer;
    }
    public async Task PublishAsync(TKey key, TData message)
    {
        await _producer.ProduceAsync(key, message);
    }
}