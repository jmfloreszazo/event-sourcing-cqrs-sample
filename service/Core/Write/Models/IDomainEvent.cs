using System;

namespace CqrsReadWriteSample.Core.Write.Models;

public interface IDomainEvent<out TAggregateId>
{
    Guid eventId { get; }
    TAggregateId aggregateId { get; }
    long aggregateVersion { get; }
    DateTime eventDate { get; }
    string eventType { get; }
}