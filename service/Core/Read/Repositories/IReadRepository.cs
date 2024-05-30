using System.Threading.Tasks;

namespace CqrsReadWriteSample.Core.Read.Repositories;

public interface IReadRepository
{
    Task UpdateReadModelOnEventPublish(string eventType);
}