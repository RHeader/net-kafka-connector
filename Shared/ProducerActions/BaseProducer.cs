using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Shared.ProducerActions;

public class BaseProducer<TKey,TData> : IDisposable
{
    private readonly IProducer<TKey,TData> _producer;
        private readonly string _topic;

    
        public BaseProducer(IOptions<ProducerConfiguration<TKey,TData>> topicOptions, IProducer<TKey,TData> producer)
        {
            _topic = topicOptions.Value.Topic;
            _producer = producer;
        }

        public async Task ProduceAsync(TKey key, TData value)
        {
            await _producer.ProduceAsync(_topic, new Message<TKey, TData> { Key = key, Value = value });
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    
}