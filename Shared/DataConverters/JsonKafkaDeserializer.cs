using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;

namespace Shared.Deserializers;

internal sealed class JsonKafkaDeserializer<T> : IDeserializer<T>
{
    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (typeof(T) == typeof(Null))
            {
                if (data.Length > 0)
                    throw new ArgumentException("The data is null not null.");
                return default;
            }

            if (typeof(T) == typeof(Ignore))
                return default;

            string stringValue = Encoding.UTF8.GetString(data);

            return JsonSerializer.Deserialize<T>(stringValue)
                   ?? throw new ArgumentNullException(nameof(T), "Deserialize output value cannot be null");
        }
    }
