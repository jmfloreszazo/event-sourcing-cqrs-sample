using System;
using CqrsReadWriteSample.Core.Write.Models;
using CqrsReadWriteSample.Domains.Orders.Write.Models;

namespace CqrsReadWriteSample.Domains.Orders.Write.Events;

public class OrderCreatedEvent : DomainEventBase<OrderAggregateId>
{
    public string customerId { get; }
    public DateTime orderDate { get; }
    public string orderStatus { get; }

    private OrderCreatedEvent()
    {
    }

    internal OrderCreatedEvent(OrderAggregateId aggregateId, string customerId, DateTime orderDate, string orderStatus)
        : base(aggregateId)
    {
        this.customerId = customerId;
        this.orderDate = orderDate;
        this.orderStatus = orderStatus;
    }

    internal OrderCreatedEvent(OrderAggregateId aggregateId, long aggregateVersion, string customerId,
        DateTime orderDate, string orderStatus)
        : base(aggregateId, aggregateVersion)
    {
        this.customerId = customerId;
        this.orderDate = orderDate;
        this.orderStatus = orderStatus;
    }

    public override IDomainEvent<OrderAggregateId> WithAggregate(OrderAggregateId aggregateId, long aggregateVersion)
    {
        return new OrderCreatedEvent(aggregateId, aggregateVersion, customerId, orderDate, orderStatus);
    }

}