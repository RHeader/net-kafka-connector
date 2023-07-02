<h2>Base description</h2>
<b>Basic connectors for easy and fast connection of your consumers and producers to Kafka instances</b>
<h2>Usage Kafka</h2>
<li> Start docker-compose </li>
<li> Go to Kafka UI [localhost:8080]() </li>

<h2>Usage producer</h2>
Basic steps:
<li> Create producer instance with `ReactiveInstance` sender object </li>

```
builder.Services.AddKafkaProducer<string, ReactiveInstance>(options =>
{
    options.Topic = "base_";
    options.BootstrapServers = "localhost:9092,localhost:9093,localhost:9094";
});
```

<li> Usage producer from DI </li>

```
private readonly IKafkaBus<string, ReactiveInstance> _bus;
await _bus.PublishAsync(Key, Body);
```

<h2>Usage Consumer</h2>
Basic steps:

<li> Create consumer instance with ReactiveInstance consumer object <br>
and ConsumerHandler </li>

```
builder.Services.AddKafkaConsumer<string, ReactiveInstance, ReactiveHandler>
(options =>
{
    options.Topic = "base_";
    options.BootstrapServers = "localhost:9092,localhost:9093,localhost:9094";
    options.GroupId = "base_group";
});
```

<li> Implemented consumer handler </li>

```
public class ReactiveHandler : IConsumerHandler<string, ReactiveInstance>
```