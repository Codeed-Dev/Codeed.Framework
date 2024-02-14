using Codeed.Framework.Tenant;
using Microsoft.Extensions.DependencyInjection;

namespace Codeed.Framework.EventBus
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterInMemoryEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddSingleton<IEventBus, InMemoryEventBus>(sp =>
            {
                return new InMemoryEventBus(services, sp.GetRequiredService<IEventBusSubscriptionsManager>());
            });

            return services;
        }

        public static IServiceCollection RegisterRequestReplyBroker(this IServiceCollection services)
        {
            services.AddSingleton<IRequestReplyBroker, MediatRRequestReplyBroker>();

            return services;
        }
    }
}
