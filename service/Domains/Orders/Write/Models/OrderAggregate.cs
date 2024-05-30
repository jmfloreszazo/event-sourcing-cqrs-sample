using System;
using System.Collections.Generic;
using CqrsReadWriteSample.Core.Write.Models;
using CqrsReadWriteSample.Domains.Orders.Write.Events;

namespace CqrsReadWriteSample.Domains.Orders.Write.Models;

public class OrderAggregate : AggregateBase<OrderAggregateId>
{
    public OrderAggregate(OrderAggregateId aggregateId, string customerId, DateTime orderDate, string orderStatus)
    {
        if (aggregateId == null) throw new ArgumentNullException(nameof(aggregateId));
        if (string.IsNullOrEmpty(customerId)) throw new ArgumentNullException(nameof(customerId));
        if (string.IsNullOrEmpty(orderStatus)) throw new ArgumentNullException(nameof(orderStatus));
        RaiseEvent(new OrderCreatedEvent(aggregateId, customerId, orderDate, orderStatus));
    }

    private OrderAggregate()
    {
        items = new List<OrderLineItem>();
    }

    internal void Apply(OrderCreatedEvent ev)
    {
        id = ev.aggregateId;
        customerId = ev.customerId;
        orderDate = ev.orderDate;
        orderStatus = ev.orderStatus;
    }

    public void AddOrderLineItem(string productId, int qty, double unitPrice)
    {
        if (string.IsNullOrEmpty(productId)) throw new ArgumentNullException(nameof(productId));
        if (qty <= 0) throw new ArgumentException("Quantity cannot be zero or negative", nameof(qty));
        if (unitPrice <= 0) throw new ArgumentException("Unit Price cannot be zero or negative", nameof(unitPrice));
        RaiseEvent(new AddOrderLineItemEvent(productId, qty, unitPrice));
    }

    internal void Apply(AddOrderLineItemEvent ev)
    {
        if (items == null) items = new List<OrderLineItem>();
        items.Add(new OrderLineItem(ev.productId, ev.qty, ev.unitPrice));
    }

    public string customerId { get; private set; }
    public DateTime orderDate { get; private set; }
    public string orderStatus { get; private set; }
    public IList<OrderLineItem> items { get; private set; }
}

public class OrderLineItem
{
    public OrderLineItem(string productId, int qty, double unitPrice)
    {
        this.productId = productId;
        this.qty = qty;
        this.unitPrice = unitPrice;
    }

    public string productId { get; private set; }
    public int qty { get; private set; }
    public double unitPrice { get; private set; }
}