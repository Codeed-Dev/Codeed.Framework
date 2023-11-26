using Codeed.Framework.Concurrency;
using Medallion.Threading;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public static class RegisterCodeedFrameworkLockerOptionsExtensions
    {
        public static void ConfigureLocker(this RegisterCodeedFrameworkOptions codeedOptions)
        {
            codeedOptions.ConfigureLocker(null);
        }

        public static void ConfigureLocker(
            this RegisterCodeedFrameworkOptions codeedOptions,
            Action<RegisterCodeedFrameworkLockerAuthenticationOptions>? configure)
        {
            var options = new RegisterCodeedFrameworkLockerAuthenticationOptions();
            if (configure is not null)
            {
                configure(options);
            }

            codeedOptions.AddServiceConfiguration(options);
        }
    }

    public class RegisterCodeedFrameworkLockerAuthenticationOptions : ICodeedServiceConfiguration
    {
        private IDistributedLockProvider? _distributedLockProvider;

        public void ConfigurePostgresDistribuited(IDistributedLockProvider distributedLockProvider)
        {
            _distributedLockProvider = distributedLockProvider;
        }

        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddScoped<ITenantLocker, TenantLocker>();

            if (_distributedLockProvider is not null)
            {
                services.AddSingleton<IDistributedLockProvider>(_distributedLockProvider);
                services.AddScoped<ILocker, DistribuitedLocker>();

                return;
            }

            services.AddScoped<ILocker, InMemoryLocker>();
        }
    }
}
