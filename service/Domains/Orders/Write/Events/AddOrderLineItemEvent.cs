using CqrsReadWriteSample.Core.Write.Models;
using CqrsReadWriteSample.Domains.Orders.Write.Models;

namespace CqrsReadWriteSample.Domains.Orders.Write.Events;

public class AddOrderLineItemEvent : DomainEventBase<OrderAggregateId>
{
    public string productId { get; }
    public int qty { get; }
    public double unitPrice { get; }

    private AddOrderLineItemEvent()
    {
    }

    internal AddOrderLineItemEvent(string productId, int qty, double unitPrice)
    {
        this.productId = productId;
        this.qty = qty;
        this.unitPrice = unitPrice;
    }

    internal AddOrderLineItemEvent(OrderAggregateId aggegateId, long aggregateVersion, string productId, int qty,
        double unitPrice)
        : base(aggegateId, aggregateVersion)
    {
        this.productId = productId;
        this.qty = qty;
        this.unitPrice = unitPrice;
    }

    public override IDomainEvent<OrderAggregateId> WithAggregate(OrderAggregateId aggregateId, long aggregateVersion)
    {
        return new AddOrderLineItemEvent(aggregateId, aggregateVersion, productId, qty, unitPrice);
    }
}