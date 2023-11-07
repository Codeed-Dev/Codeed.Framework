using Codeed.Framework.Domain;
using Codeed.Framework.Tenant;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

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


        public override Task Publish<TEvent>(TEvent @event)
        {
            return Publish(@event.ObjectToList());
        }

        public override async Task Publish<TEvent>(IEnumerable<TEvent> events)
        {
            if (events is null || !events.Any())
                return;

            var eventsGrouped = events.GroupBy(e => new { EventKey = _eventBusSubscriptionsManager.GetEventKey(e), Tenant = e is ITenantEvent tenantEvent ? tenantEvent.Tenant : ""});
            var publishEventErrors = new List<Exception>();
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            using (var serviceScope = serviceProvider.CreateScope())
            {
                foreach (var groupedEvent in eventsGrouped)
                {
                    var tenantService = serviceScope.ServiceProvider.GetRequiredService<ITenantService>();
                    tenantService.SetTenant(groupedEvent.Key.Tenant);
                    var eventName = groupedEvent.Key.EventKey;

                    if (!_eventBusSubscriptionsManager.HasSubscriptionsForEvent(eventName))
                    {
                        continue;
                    }

                    var handlers = _eventBusSubscriptionsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in handlers)
                    {
                        var handler = ActivatorUtilities.CreateInstance(serviceScope.ServiceProvider, subscription.HandlerType);
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
                        var notificationHandler = typeof(INotificationHandler<>).MakeGenericType(eventType);

                        foreach (var @event in groupedEvent)
                        {
                            if (@event is ITenantEvent tenantEvent)
                            {
                                tenantEvent.Tenant = tenantService.Tenant;
                            }

                            try
                            {
                                var handleResult = notificationHandler.GetMethod(nameof(IEventHandler<Event>.Handle))?.Invoke(handler, new object[] { @event, CancellationToken.None });

                                if (handleResult is Task task)
                                    await task.ConfigureAwait(false);
                            }
                            catch (Exception e)
                            {
                                publishEventErrors.Add(e);
                            }
                        }
                    }
                }
            }

            var firstError = publishEventErrors.FirstOrDefault();
            if (firstError != null)
            {
                throw firstError;
            }
        }

        public override void Subscribe<TEvent, TEventHandler>()
        {
            _eventBusSubscriptionsManager.AddSubscription<TEvent, TEventHandler>();
        }
    }
}
