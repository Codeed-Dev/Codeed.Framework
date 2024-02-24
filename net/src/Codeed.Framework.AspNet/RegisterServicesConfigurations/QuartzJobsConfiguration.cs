using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.AspNetCore;
using System.Reflection;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public static class RegisterCodeedFrameworkQuartzConfigurationExtensions
    {
        public static void ConfigureQuartz(this RegisterCodeedFrameworkOptions codeedOptions, Action<IServiceCollectionQuartzConfigurator>? configure = null)
        {
            var options = new QuartzJobsConfiguration(configure);
            codeedOptions.AddServiceConfiguration(options);
        }
    }

    public class QuartzJobsConfiguration : ICodeedServiceConfiguration
    {
        private readonly Action<IServiceCollectionQuartzConfigurator>? _configure;

        public QuartzJobsConfiguration(Action<IServiceCollectionQuartzConfigurator>? configure = null)
        {
            _configure = configure;
        }

        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddQuartz(_configure);

            services.AddQuartzServer(options =>
            {
                options.WaitForJobsToComplete = true;
            });
        }
    }
}
