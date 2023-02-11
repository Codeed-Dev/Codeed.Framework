using Codeed.Framework.EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Codeed.Framework.AspNet.EventBus
{
    public static class IApplicationBuilderExtensions
    {
        public static void SubscribeEventHandlers(this IApplicationBuilder app)
        {
            var assemblies = app.ApplicationServices.GetRequiredService<IEnumerable<Assembly>>();
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            var eventHandlers = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var assemblyEventHandlers = assembly.GetTypes().Where(HasEventHandler);
                eventHandlers.AddRange(assemblyEventHandlers);
            }

            foreach (var eventHandler in eventHandlers)
            {
                foreach (var interfaceType in eventHandler.GetInterfaces().Where(IsEventHandler))
                {
                    var eventType = interfaceType.GetGenericArguments()[0];
                    eventBus.GetType().GetMethod(nameof(IEventBus.Subscribe))
                                      .MakeGenericMethod(eventType, eventHandler)
                                      .Invoke(eventBus, null);

                }
            }
        }

        private static bool HasEventHandler(System.Type type)
        {
            if (type.IsAbstract)
                return false;

            return type.GetInterfaces()
                       .ToList()
                       .Exists(IsEventHandler);
        }

        private static bool IsEventHandler(System.Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEventHandler<>);
        }
    }
}
