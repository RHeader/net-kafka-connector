<h2>Base description</h2>
**Basic connectors for easy and fast connection of your consumers and producers to Kafka instances**
<h2>Usage Kafka</h2>
* Start docker-compose
* Go to Kafka UI [localhost:8080]()

<h2>Usage producer</h2>
Basic steps:
* Create producer instance with `ReactiveInstance` sender object
```
builder.Services.AddKafkaProducer<string, ReactiveInstance>(options =>
{
    options.Topic = "base_";
    options.BootstrapServers = "localhost:9092,localhost:9093,localhost:9094";
});
```
* Usage producer from DI
```
private readonly IKafkaBus<string, ReactiveInstance> _bus;
await _bus.PublishAsync(Key, Body);
```

<h2>Usage Consumer</h2>
Basic steps:
* Create consumer instance with ReactiveInstance consumer object <br>
and ConsumerHandler
```
builder.Services.AddKafkaConsumer<string, ReactiveInstance, ReactiveHandler>
(options =>
{
    options.Topic = "base_";
    options.BootstrapServers = "localhost:9092,localhost:9093,localhost:9094";
    options.GroupId = "base_group";
});
```
* Implemented consumer handler

```
public class ReactiveHandler : IConsumerHandler<string, ReactiveInstance>
```