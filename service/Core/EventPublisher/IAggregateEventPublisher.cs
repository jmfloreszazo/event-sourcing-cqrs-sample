using System.Collections.Generic;
using System.Threading.Tasks;

namespace CqrsReadWriteSample.Core.EventPublisher;

public interface IAggregateEventPublisher<TAggregateId>
{
    Task PublishEvent(string eventType, string aggregateId, IDictionary<string, object> eventData);
}