using System;
using System.Linq;
using System.Threading.Tasks;
using CqrsReadWriteSample.Core.Read.Repositories;
using CqrsReadWriteSample.Domains.Orders.Read.Models;
using EventSourcingCQRS.Infrastructure.Persistance;

namespace CqrsReadWriteSample.Infrastructure.Persistance
{
    public class InMemoryReadRepository : IReadRepository
    {
        public async Task UpdateReadModelOnEventPublish(string eventType)
        {
            await Task.Delay(1);
            var latestPublishedEvent = InMemoryReadPersistance.publishedEvents.OrderByDescending(x => x.eventDate).FirstOrDefault();
            switch (latestPublishedEvent.eventType)
            {
                case "OrderCreatedEvent":
                    if (InMemoryReadPersistance.readOrders.Where(x => x.aggregateId == latestPublishedEvent.aggregateId.ToString()).FirstOrDefault() == null)
                        InMemoryReadPersistance.readOrders.Add(new Order(
                            latestPublishedEvent.aggregateId.ToString(),
                            latestPublishedEvent.eventData["customerId"].ToString(),
                            (DateTime)latestPublishedEvent.eventData["orderDate"],
                            latestPublishedEvent.eventData["orderStatus"].ToString()));
                    break;
                case "AddOrderLineItemEvent":
                    {
                        var order = InMemoryReadPersistance.readOrders.Where(x => x.aggregateId == latestPublishedEvent.aggregateId).FirstOrDefault();
                        if (order != null)
                        {
                            var updatedOrder = CopyOrder(order);
                            updatedOrder.items.Add(new OrderLineItem()
                            {
                                productId = latestPublishedEvent.eventData["productId"].ToString(),
                                qty = (int)latestPublishedEvent.eventData["qty"],
                                unitPrice = (double)latestPublishedEvent.eventData["unitPrice"]
                            });
                            InMemoryReadPersistance.readOrders.Remove(order);
                            InMemoryReadPersistance.readOrders.Add(updatedOrder);
                        }
                    }
                    break;
                case "RemoveOrderLineItemEvent":
                    {
                        var order = InMemoryReadPersistance.readOrders.FirstOrDefault(x => x.aggregateId == latestPublishedEvent.aggregateId);
                        if (order != null)
                        {
                            var updatedOrder = CopyOrder(order);
                            var lineItemToRemove = updatedOrder.items.FirstOrDefault(item => item.productId == latestPublishedEvent.eventData["productId"].ToString());
                            if (lineItemToRemove != null)
                            {
                                updatedOrder.items.Remove(lineItemToRemove);
                                InMemoryReadPersistance.readOrders.Remove(order);
                                InMemoryReadPersistance.readOrders.Add(updatedOrder);
                            }
                        }
                    }
                    break;
            }
        }

        private static Order CopyOrder(Order order)
        {
            var updatedOrder = new Order(order.aggregateId, order.customerId, order.orderDate, order.orderStatus);
            if (order.items != null && order.items.Count > 0) updatedOrder.items = order.items;
            return updatedOrder;
        }
    }
}