using System;
using System.Collections.Generic;
using System.Text;
using CqrsReadWriteSample.Core.Write.Models;
using CqrsReadWriteSample.Domains.Orders.Read.Models;

namespace EventSourcingCQRS.Infrastructure.Persistance
{
    public class InMemoryWritePersistance<TAggregateId>
    {
        public static IList<IDomainEvent<TAggregateId>> domainEvents = new List<IDomainEvent<TAggregateId>>();

    }

    public class InMemoryReadPersistance
    {
        public static IList<MemoryPersistanceDomainEventModel> publishedEvents = new List<MemoryPersistanceDomainEventModel>();
        public static IList<Order> readOrders = new List<Order>();
    }
}