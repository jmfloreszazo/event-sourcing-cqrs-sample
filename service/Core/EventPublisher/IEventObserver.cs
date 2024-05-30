using System.Threading.Tasks;

namespace CqrsReadWriteSample.Core.EventPublisher;

public interface IEventObserver
{
    Task Update(string eventType);
}
