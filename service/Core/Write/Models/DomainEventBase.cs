using System;
using System.Collections.Generic;

namespace CqrsReadWriteSample.Core.Write.Models;

public abstract class DomainEventBase<TAggregateId> : IDomainEvent<TAggregateId>,
    IEquatable<DomainEventBase<TAggregateId>>
{
    protected DomainEventBase()
    {
        eventId = Guid.NewGuid();
        eventDate = DateTime.Now;
        eventType = GetType().Name;
    }

    protected DomainEventBase(TAggregateId aggregateId) : this()
    {
        this.aggregateId = aggregateId;
    }

    protected DomainEventBase(TAggregateId aggregateId, long aggregateVersion) : this(aggregateId)
    {
        this.aggregateVersion = aggregateVersion;
    }

    public Guid eventId { get; }
    public TAggregateId aggregateId { get; }
    public long aggregateVersion { get; }
    public DateTime eventDate { get; }
    public string eventType { get; }

    public override bool Equals(object obj)
    {
        return base.Equals(obj as DomainEventBase<TAggregateId>);
    }

    public bool Equals(DomainEventBase<TAggregateId> other)
    {
        return other != null && eventId.Equals(other.eventId);
    }

    public override int GetHashCode()
    {
        return eventId.GetHashCode();
    }

    public abstract IDomainEvent<TAggregateId> WithAggregate(TAggregateId aggregateId, long aggregateVersion);
}