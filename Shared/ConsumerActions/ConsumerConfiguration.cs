using Confluent.Kafka;

namespace Shared.ConsumerActions;

public class ConsumerConfiguration<TKey,TData> :ConsumerConfig
{     
        public string Topic { get; set; }
        public ConsumerConfiguration()
        {
            AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
            EnableAutoOffsetStore = false;
        }
}