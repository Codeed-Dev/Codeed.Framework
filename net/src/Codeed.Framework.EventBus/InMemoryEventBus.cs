using Codeed.Framework.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Codeed.Framework.EventBus
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly IServiceScope _serviceScope;

        public InMemoryEventBus(IServiceCollection serviceCollection, IServiceScope serviceScope)
        {
            _serviceCollection = serviceCollection;
            _serviceScope = serviceScope;
        }

        public void Publish<TEvent>(TEvent @event)
            where TEvent : Event
        {
            var handlers = _serviceScope.ServiceProvider.GetServices<IEventHandler<TEvent>>();

            var tasks = new List<Task>();
            foreach (var handler in handlers)
            {
                tasks.Add(handler.Handle(@event));
            }

            Task.WhenAll(tasks);
        }

        public void Subscribe<Tevent, TEventHandler>()
            where Tevent : Event
            where TEventHandler : IEventHandler<Tevent>
        {
            _serviceCollection.AddScoped(typeof(TEventHandler));
        }
    }
}
