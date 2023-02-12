using Codeed.Framework.Services.BackgroundTasks;
using CodeedMeta.SharedContext.BackgroundTasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public static class RegisterCodeedFrameworkBackgroundTasksConfigurationExtensions
    {
        public static void ConfigureBackgroundTasks(this RegisterCodeedFrameworkOptions codeedOptions)
        {
            codeedOptions.ConfigureBackgroundTasks(null);
        }

        public static void ConfigureBackgroundTasks(
            this RegisterCodeedFrameworkOptions codeedOptions, 
            Action<RegisterBackgroundTasksConfiguration> configure)
        {
            var options = new RegisterBackgroundTasksConfiguration();
            if (configure != null)
            {
                configure(options);
            }

            codeedOptions.AddServiceConfiguration(options);
        }
    }

    public class RegisterBackgroundTasksConfiguration : ICodeedServiceConfiguration
    {
        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHostedService<BackgroundQueueHostedService>();
        }
    }
}