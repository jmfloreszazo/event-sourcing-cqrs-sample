using System.Collections.Generic;

namespace CqrsReadWriteSample.Core.Write.Models;

public interface IEventSourcingAggregateRoot<TAggregateId>
{
    long version { get; }
    void ApplyEvent(IDomainEvent<TAggregateId> @event, long version);
    IEnumerable<IDomainEvent<TAggregateId>> GetUncommittedEvents();
    void ClearUncommittedEvents();
}