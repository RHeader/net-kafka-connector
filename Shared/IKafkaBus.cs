namespace Shared;

public interface IKafkaBus<TKey,TData>
{
    Task PublishAsync(TKey key, TData message);
}