using Codeed.Framework.Domain;
using Codeed.Framework.Tenant;
using Microsoft.Extensions.DependencyInjection;

namespace Codeed.Framework.EventBus
{
    public class InMemoryEventBus : BaseEventBus
    {
        private readonly IEventBusSubscriptionsManager _eventBusSubscriptionsManager;
        private readonly IServiceCollection _serviceCollection;

        public InMemoryEventBus(
            IServiceCollection serviceCollection, 
            IEventBusSubscriptionsManager eventBusSubscriptionsManager)
        {
            _eventBusSubscriptionsManager = eventBusSubscriptionsManager;
            _serviceCollection = serviceCollection;
        }


        public override async Task Publish<TEvent>(TEvent @event)
        {
            var eventName = _eventBusSubscriptionsManager.GetEventKey(@event);
            if (!_eventBusSubscriptionsManager.HasSubscriptionsForEvent(eventName))
            {
                return;
            }

            var handlers = _eventBusSubscriptionsManager.GetHandlersForEvent(eventName);

            foreach (var subscription in handlers)
            {
                var serviceProvider = _serviceCollection.BuildServiceProvider();
                using (var serviceScope = serviceProvider.CreateScope())
                {
                    if (@event is ITenantEvent tenantEvent)
                    {
                        var tenantServiceScope = serviceProvider.GetRequiredService<ITenantService>();
                        tenantServiceScope.SetTenant(tenantEvent.Tenant);
                    }

                    var handler = ActivatorUtilities.CreateInstance(serviceProvider, subscription.HandlerType);
                    if (handler is null)
                    {
                        continue;
                    }

                    var eventType = _eventBusSubscriptionsManager.GetEventTypeByName(eventName);
                    if (eventType is null)
                    {
                        continue;
                    }

                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                    await Task.Yield();
                    var handleResult = concreteType.GetMethod("Handle")?.Invoke(handler, new object[] { @event });

                    if (handleResult is Task task)
                        await task;
                }
            }
        }

        public override void Subscribe<TEvent, TEventHandler>()
        {
            _eventBusSubscriptionsManager.AddSubscription<TEvent, TEventHandler>();
        }
    }
}
