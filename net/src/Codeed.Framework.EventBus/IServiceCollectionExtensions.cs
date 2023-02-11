using Microsoft.Extensions.DependencyInjection;

namespace Codeed.Framework.EventBus
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterInMemoryEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, InMemoryEventBus>(sp =>
            {
                return new InMemoryEventBus(services, new InMemoryEventBusSubscriptionsManager());
            });

            return services;
        }
    }
}
