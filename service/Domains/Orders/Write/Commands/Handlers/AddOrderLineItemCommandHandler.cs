using System.Threading.Tasks;
using CqrsReadWriteSample.Core.EventPublisher;
using CqrsReadWriteSample.Core.Write.Commands.Handlers;
using CqrsReadWriteSample.Core.Write.Repositories;
using CqrsReadWriteSample.Domains.Orders.Write.Models;
using CqrsReadWriteSample.Infrastructure.Persistance;
using EventSourcingCQRS.Infrastructure.Persistance;

namespace CqrsReadWriteSample.Domains.Orders.Write.Commands.Handlers;

public class AddOrderLineItemCommandHandler : ICommandHandler<AddOrderLineItemCommand>
{
    public async Task Handle(AddOrderLineItemCommand command)
    {
        IAggregateEventPublisher<OrderAggregateId> publisher = new InMemoryDomainEventPublisher<OrderAggregateId>();
        IWriteRepository<OrderAggregate, OrderAggregateId> repo =
            new InMemoryWriteRepository<OrderAggregate, OrderAggregateId>(publisher);
        var orderAggregate = await repo.GetByIdAsync(command.orderAggregateId);
        orderAggregate.AddOrderLineItem(command.productId, command.qty, command.unitPrice);
        await repo.SaveAsync(orderAggregate);
    }
}