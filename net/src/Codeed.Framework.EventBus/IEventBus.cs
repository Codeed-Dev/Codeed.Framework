using Codeed.Framework.Domain;
using Codeed.Framework.Tenant;

namespace Codeed.Framework.EventBus
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event)
             where TEvent : IEvent;

        Task Publish<TEvent>(IEnumerable<TEvent> @events)
             where TEvent : IEvent;

        Task Publish<TEvent>(TEvent @event, ITenantService tenantService)
            where TEvent : ITenantEvent;

        Task Publish<TEvent>(IEnumerable<TEvent> @events, ITenantService tenantService)
            where TEvent : ITenantEvent;

        void Subscribe<TEvent, TEventHandler>()
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>;
    }
}
