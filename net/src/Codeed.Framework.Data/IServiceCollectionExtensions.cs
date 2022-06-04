using Codeed.Framework.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Influencer.Core.Data
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRepository<TInterface, TImplementation, TEntity>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface, IRepository<TEntity>
            where TEntity : Entity, IAggregateRoot
        {
            services.AddScoped<TInterface, TImplementation>();
            services.AddScoped<IRepository<TEntity>, TImplementation>();
            return services;
        }
    }
}
