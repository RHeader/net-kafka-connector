namespace Shared.ConsumerActions;

public interface IConsumerHandler<in TKey, in TData>
{
    Task HandleAsync(TKey key, TData value);
}