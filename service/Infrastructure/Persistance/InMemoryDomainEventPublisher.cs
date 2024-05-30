using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CqrsReadWriteSample.Core.EventPublisher;
using EventSourcingCQRS.Infrastructure.Persistance;

namespace CqrsReadWriteSample.Infrastructure.Persistance
{
    public class InMemoryDomainEventPublisher<TAggregateId> : IAggregateEventPublisher<TAggregateId>
    {
        private EventPublisher _eventPublisher;
        public InMemoryDomainEventPublisher()
        {
            _eventPublisher = new EventPublisher();
            _ = new EventObserver(new InMemoryReadRepository(), _eventPublisher);
        }
        public async Task PublishEvent(string eventType, string aggregateId, IDictionary<string, object> eventData)
        {
            InMemoryReadPersistance.publishedEvents.Add(new MemoryPersistanceDomainEventModel()
            {
                eventType = eventType,
                eventDate = DateTime.Now,
                aggregateId = aggregateId,
                eventData = eventData
            });
            await _eventPublisher.setEventPublished(eventType);
        }
    }
}