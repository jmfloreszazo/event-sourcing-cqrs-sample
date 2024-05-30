using System;
using CqrsReadWriteSample.Core.Write.Models;

namespace CqrsReadWriteSample.Domains.Orders.Write.Models;

public class OrderAggregateId : IAggregateId
{
    public OrderAggregateId()
    {
        id = Guid.NewGuid();
    }

    public OrderAggregateId(string aggregateIdString)
    {
        id = Guid.Parse(aggregateIdString);
    }

    public Guid id { get; }

    public string IdAsString()
    {
        return id.ToString();
    }

    public override string ToString()
    {
        return IdAsString();
    }
}