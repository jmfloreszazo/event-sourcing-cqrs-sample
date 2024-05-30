namespace CqrsReadWriteSample.Core.Write.Models;

public interface IAggregateRoot<out TId>
{
    TId id { get; }
}