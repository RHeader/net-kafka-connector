using Confluent.Kafka;

namespace Shared.ProducerActions;

public class ProducerConfiguration<TKey, TData>:ProducerConfig
{
    public string Topic { get; set; }
}