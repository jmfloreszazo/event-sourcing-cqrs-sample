using System.Threading.Tasks;
using CqrsReadWriteSample.Core.Read.Repositories;

namespace CqrsReadWriteSample.Core.EventPublisher;

public class EventObserver : IEventObserver
{
    private readonly IReadRepository _repository;

    public EventObserver(IReadRepository repository, IEventPublisher eventPublisher)
    {
        _repository = repository;
        eventPublisher.RegisterObserver(this);
    }

    public async Task Update(string eventType)
    {
        await _repository.UpdateReadModelOnEventPublish(eventType);
    }
}