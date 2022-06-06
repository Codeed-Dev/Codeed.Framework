using Microsoft.Extensions.DependencyInjection;

namespace Codeed.Framework.Environment.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection RegisterEnvironment(this IServiceCollection services)
        {
            services.AddSingleton<IEnvironment, Environment>();
            return services;
        }
    }
}
