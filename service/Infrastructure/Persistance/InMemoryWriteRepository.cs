using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CqrsReadWriteSample.Core.EventPublisher;
using CqrsReadWriteSample.Core.Write.Models;
using CqrsReadWriteSample.Core.Write.Repositories;

namespace EventSourcingCQRS.Infrastructure.Persistance
{
    public class InMemoryWriteRepository<TAggregate, TAggregateId> : IWriteRepository<TAggregate, TAggregateId> where TAggregate : IAggregateRoot<TAggregateId>
    {
        IAggregateEventPublisher<TAggregateId> eventPublisher;

        public InMemoryWriteRepository(IAggregateEventPublisher<TAggregateId> eventPublisher)
        {
            this.eventPublisher = eventPublisher;
        }

        public async Task SaveAsync(TAggregate aggregate)
        {
            try
            {
                IEventSourcingAggregateRoot<TAggregateId> aggregatePersistence = (IEventSourcingAggregateRoot<TAggregateId>)aggregate;
                foreach (var @event in aggregatePersistence.GetUncommittedEvents())
                {
                    await AppendEventAsAsync(@event);
                    await eventPublisher.PublishEvent(@event.GetType().Name, @event.aggregateId.ToString(), GetEventAsMap(@event));
                }
                aggregatePersistence.ClearUncommittedEvents();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private async Task AppendEventAsAsync(IDomainEvent<TAggregateId> @event)
        {
            await Task.Delay(1);
            InMemoryWritePersistance<TAggregateId>.domainEvents.Add(@event);
        }

        private MemoryPersistanceEventModel GetMemoryPersistanceEventModel(IDomainEvent<TAggregateId> @event)
        {
            var memoryEvent = new MemoryPersistanceEventModel();
            memoryEvent.eventId = @event.eventId;
            memoryEvent.aggregateId = @event.aggregateId.ToString();
            memoryEvent.aggregateVersion = @event.aggregateVersion;
            memoryEvent.eventDate = @event.eventDate;
            memoryEvent.eventType = @event.GetType().Name;
            memoryEvent.eventMap = GetEventAsMap(@event);
            return memoryEvent;
        }

        private IDictionary<string, object> GetEventAsMap(IDomainEvent<TAggregateId> @event)
        {
            var map = new Dictionary<string, object>();
            var properties = @event.GetType().GetProperties();
            foreach (var p in properties) if (!GetIgnoreProperties().Contains(p.Name)) map.Add(p.Name, p.GetValue(@event, null));
            return map;
        }

        private HashSet<string> GetIgnoreProperties()
        {
            var result = new HashSet<string>();
            result.Add("eventId");
            result.Add("AggregateId");
            result.Add("aggregateVersion");
            result.Add("eventDate");
            result.Add("eventType");
            return result;
        }

        public async Task<TAggregate> GetByIdAsync(string id)
        {
            try
            {
                await Task.Delay(1);
                var aggregate = CreateEmptyAggregate();
                IEventSourcingAggregateRoot<TAggregateId> aggregatePersistence = (IEventSourcingAggregateRoot<TAggregateId>)aggregate;

                foreach (var @event in InMemoryWritePersistance<TAggregateId>.domainEvents.Where(x => x.aggregateId.ToString() == id).ToList())
                {
                    aggregatePersistence.ApplyEvent(@event, @event.aggregateVersion);
                }
                return aggregate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private TAggregate CreateEmptyAggregate()
        {
            return (TAggregate)typeof(TAggregate)
                    .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                        null, new Type[0], new ParameterModifier[0])
                    .Invoke(new object[0]);
            // return Activator.CreateInstance<TAggregate>();
        }
    }
}