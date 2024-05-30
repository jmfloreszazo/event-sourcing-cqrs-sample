using System.Threading.Tasks;

namespace CqrsReadWriteSample.Core.EventPublisher;

public interface IEventPublisher
{
    void RegisterObserver(IEventObserver observer);
    void RemoveObserver(IEventObserver observer);
    Task NotifyObservers();
}