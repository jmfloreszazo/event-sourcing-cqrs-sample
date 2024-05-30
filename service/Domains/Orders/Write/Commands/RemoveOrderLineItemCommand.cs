namespace CqrsReadWriteSample.Domains.Orders.Write.Commands;

public class RemoveOrderLineItemCommand
{
    public int OrderId { get; set; }
    public int LineItemId { get; set; }
}
