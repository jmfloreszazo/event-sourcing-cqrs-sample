namespace CqrsReadWriteSample.Core.Write.Models
{
    public interface IAggregateId
    {
        string IdAsString();
    }
}