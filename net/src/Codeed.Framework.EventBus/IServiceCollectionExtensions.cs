using Codeed.Framework.Tenant;
using Microsoft.Extensions.DependencyInjection;

namespace Codeed.Framework.EventBus
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterInMemoryEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, InMemoryEventBus>(sp =>
            {
                var tenantService = sp.GetRequiredService<ITenantService>();
                return new InMemoryEventBus(services, new InMemoryEventBusSubscriptionsManager(), tenantService);
            });

            return services;
        }
    }
}
