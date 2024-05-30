namespace CqrsReadWriteSample.Domains.Orders.Write.Commands;

public class AddOrderLineItemCommand
{
    public string orderAggregateId { get; set; }
    public string productId { get; set; }
    public int qty { get; set; }
    public double unitPrice { get; set; }
}