using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.ConsumerActions;
using Shared.DataConverters;
using Shared.ProducerActions;

namespace Shared;

public static class RegisterServices
{
    public static IServiceCollection AddKafkaMessageBus(this IServiceCollection serviceCollection)
        => serviceCollection.AddSingleton(typeof(IKafkaBus<,>), typeof(KafkaMessageBus<,>));

    public static IServiceCollection AddKafkaConsumer<TKey, TData, THandler>(this IServiceCollection services,
        Action<ConsumerConfiguration<TKey, TData>> configAction) where THandler : class, IConsumerHandler<TKey, TData>
    {
        services.AddScoped<IConsumerHandler<TKey, TData>, THandler>();

        services.AddHostedService<ConsumerBackgroundInstance<TKey, TData>>();

        services.Configure(configAction);

        return services;
    }

    public static IServiceCollection AddKafkaProducer<TKey, TData>(this IServiceCollection services,
        Action<ProducerConfiguration<TKey, TData>> configAction)
    {
        services.AddCoreProducer<TKey, TData>();

        services.AddSingleton<BaseProducer<TKey, TData>>();

        services.Configure(configAction);

        return services;
    }

    private static IServiceCollection AddCoreProducer<TKey, TData>(this IServiceCollection services)
    {
        services.AddSingleton(
            sp =>
            {
                var config = sp.GetRequiredService<IOptions<ProducerConfiguration<TKey, TData>>>();

                var builder = new ProducerBuilder<TKey, TData>(config.Value).SetValueSerializer(new ByteKafkaSerializer<TData>());

                return builder.Build();
            });

        return services;
    }
}