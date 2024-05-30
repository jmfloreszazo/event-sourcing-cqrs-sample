using System.Collections.Generic;
using System.Threading.Tasks;

namespace CqrsReadWriteSample.Core.EventPublisher
{
    public class EventPublisher : IEventPublisher
    {
        private List<IEventObserver> _observers = new List<IEventObserver>();
        private string _eventType { get; set; }
        public async Task setEventPublished(string eventType)
        {
            _eventType = eventType;
            await NotifyObservers();
        }
        public async Task NotifyObservers()
        {
            foreach (var o in _observers) await o.Update(_eventType);
        }

        public void RegisterObserver(IEventObserver observer) => _observers.Add(observer);

        public void RemoveObserver(IEventObserver observer) => _observers.Remove(observer);
    }
}