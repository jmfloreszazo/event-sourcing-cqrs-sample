using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CqrsReadWriteSample.Core.Read.Repositories;
using CqrsReadWriteSample.Core.Write.Commands.Handlers;
using CqrsReadWriteSample.Domains.Orders.Write.Commands;
using CqrsReadWriteSample.Infrastructure.Persistance;
using EventSourcingCQRS.Infrastructure.Persistance;
using Xunit;

namespace CqrsReadWriteSample.UnitTest;

public class OrdersUnitTests
{

    [Fact]
    public async Task CreateOrderCommand_ShouldCreateNewOrderAndPublishOrderCreatedEvent()
    {
        var commandProcessor = new CommandProcessor();

        await commandProcessor.ExecuteCommand(new CreateOrderCommand
        {
            customerId = "customer 1",
            orderDate = DateTime.Now,
            orderStatus = "NEW"
        });

        var lastPublishedEvent = InMemoryReadPersistance.publishedEvents.LastOrDefault();
        Assert.NotNull(lastPublishedEvent);
        Assert.Equal("OrderCreatedEvent", lastPublishedEvent.eventType);
    }

    [Fact]
    public async Task OrderCreatedEvent_ShouldUpdateOrderReadModel()
    {
        var latestPublishedEvent = InMemoryReadPersistance.publishedEvents.MaxBy(x => x.eventDate);

        if (latestPublishedEvent != null && IsOrderRelatedEvent(latestPublishedEvent.eventType))
        {
            IReadRepository repo = new InMemoryReadRepository();
            await repo.UpdateReadModelOnEventPublish(latestPublishedEvent.eventType);

            var readOrder = InMemoryReadPersistance.readOrders
                .FirstOrDefault(x => x.aggregateId == latestPublishedEvent.aggregateId);
            Assert.NotNull(readOrder);
        }
    }

    [Fact]
    public async Task AddOrderLineItemCommand_ShouldUpdateAggregateAndPublishAddOrderLineItemEvent()
    {
        var orderCreatedEvent = InMemoryReadPersistance.publishedEvents
            .Where(x => x.eventType == "OrderCreatedEvent").MaxBy(x => x.eventDate);

        if (orderCreatedEvent != null)
        {
            var commandProcessor = new CommandProcessor();
            await commandProcessor.ExecuteCommand(new AddOrderLineItemCommand
            {
                orderAggregateId = orderCreatedEvent.aggregateId,
                productId = Guid.NewGuid().ToString(),
                qty = 10,
                unitPrice = 100.0
            });

            var addItemEvent = InMemoryReadPersistance.publishedEvents
                .FirstOrDefault(x => x.eventType == "AddOrderLineItemEvent");
            Assert.NotNull(addItemEvent);

            var readOrder = InMemoryReadPersistance.readOrders
                .FirstOrDefault(x => x.aggregateId == orderCreatedEvent.aggregateId);
            Assert.NotNull(readOrder);
            Assert.NotEmpty(readOrder.items);
        }
    }

    //[Fact]
    //public async Task RemoveOrderLineItemCommand_ShouldUpdateAggregateAndPublishRemoveOrderLineItemEvent()
    //{
    //    var orderCreatedEvent = InMemoryReadPersistance.publishedEvents
    //        .Where(x => x.eventType == "OrderCreatedEvent").MaxBy(x => x.eventDate);

    //    if (orderCreatedEvent != null)
    //    {
    //        var commandProcessor = new CommandProcessor();
    //        await commandProcessor.ExecuteCommand(new RemoveOrderLineItemCommand
    //        {
    //            LineItemId = 1,
    //            OrderId = 1
    //        });

    //        var removeItemEvent = InMemoryReadPersistance.publishedEvents
    //            .FirstOrDefault(x => x.eventType == "RemoveOrderLineItemEvent");
    //        Assert.NotNull(removeItemEvent);

    //        var readOrder = InMemoryReadPersistance.readOrders
    //            .FirstOrDefault(x => x.aggregateId == orderCreatedEvent.aggregateId);
    //        Assert.Null(readOrder);
    //        Assert.Empty(readOrder.items);
    //    }
    //}

    private static bool IsOrderRelatedEvent(string eventType)
    {
        var orderRelatedEvents = new HashSet<string>
        {
            "OrderCreatedEvent",
            "AddOrderLineItemEvent",
            "RemoveOrderLineItemEvent"
        };

        return orderRelatedEvents.Contains(eventType);
    }
}
