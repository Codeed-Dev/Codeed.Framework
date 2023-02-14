using Codeed.Framework.Domain;
using Codeed.Framework.Tenant;

namespace Codeed.Framework.EventBus
{
    public abstract class BaseEventBus : IEventBus
    {
        public abstract Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;

        public Task Publish<TEvent>(TEvent @event, ITenantService tenantService) where TEvent : ITenantEvent
        {
            @event.Tenant = tenantService.Tenant;
            return Publish(@event);
        }

        public abstract void Subscribe<TEvent, TEventHandler>()
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>;
    }
}
