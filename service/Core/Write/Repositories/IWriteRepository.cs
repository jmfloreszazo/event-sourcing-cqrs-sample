using System.Threading.Tasks;
using CqrsReadWriteSample.Core.Write.Models;

namespace CqrsReadWriteSample.Core.Write.Repositories
{
    public interface IWriteRepository<TAggregate, TAggregateId> where TAggregate : IAggregateRoot<TAggregateId>
    {
        Task SaveAsync(TAggregate aggregate);
        Task<TAggregate> GetByIdAsync(string id);
    }
}