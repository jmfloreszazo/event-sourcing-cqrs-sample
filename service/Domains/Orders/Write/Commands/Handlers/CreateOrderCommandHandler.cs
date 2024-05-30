using System.Threading.Tasks;
using CqrsReadWriteSample.Core.EventPublisher;
using CqrsReadWriteSample.Core.Write.Commands.Handlers;
using CqrsReadWriteSample.Core.Write.Repositories;
using CqrsReadWriteSample.Domains.Orders.Write.Models;
using CqrsReadWriteSample.Infrastructure.Persistance;
using EventSourcingCQRS.Infrastructure.Persistance;

namespace CqrsReadWriteSample.Domains.Orders.Write.Commands.Handlers;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
{
    public async Task Handle(CreateOrderCommand command)
    {
        var aggregateId = new OrderAggregateId();
        var order = new OrderAggregate(aggregateId, command.customerId, command.orderDate, command.orderStatus);
        IAggregateEventPublisher<OrderAggregateId> publisher = new InMemoryDomainEventPublisher<OrderAggregateId>();
        IWriteRepository<OrderAggregate, OrderAggregateId> repo =
            new InMemoryWriteRepository<OrderAggregate, OrderAggregateId>(publisher);
        await repo.SaveAsync(order);
    }
}