using System;
using System.Collections.Generic;

namespace CqrsReadWriteSample.Domains.Orders.Read.Models;

public class Order : BaseReadEntity
{
    public Order(string id, string customerId, DateTime orderDate, string orderStatus)
    {
        aggregateId = id;
        this.customerId = customerId;
        this.orderDate = orderDate;
        this.orderStatus = orderStatus;
        items = new List<OrderLineItem>();
    }

    public string customerId { get; set; }
    public DateTime orderDate { get; set; }
    public string orderStatus { get; set; }
    public IList<OrderLineItem> items { get; set; }

}

public class OrderLineItem
{
    public string productId { get; set; }
    public int qty { get; set; }
    public double unitPrice { get; set; }

}