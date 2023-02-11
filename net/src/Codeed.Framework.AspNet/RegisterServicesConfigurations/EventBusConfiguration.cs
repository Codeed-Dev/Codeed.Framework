using Codeed.Framework.EventBus;
using Codeed.Framework.EventBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public static class RegisterCodeedFrameworkEventBusConfigurationExtensions
    {
        public static void ConfigureEventBus(this RegisterCodeedFrameworkOptions codeedOptions)
        {
            codeedOptions.ConfigureEventBus(null);
        }

        public static void ConfigureEventBus(
            this RegisterCodeedFrameworkOptions codeedOptions, 
            Action<RegisterEventBusConfiguration> configure)
        {
            var options = new RegisterEventBusConfiguration();
            if (configure != null)
            {
                configure(options);
            }

            codeedOptions.AddServiceConfiguration(options);
        }
    }

    public class RegisterEventBusConfiguration : ICodeedServiceConfiguration
    {
        private RabbitMQConfiguration _rabbitMQConfiguration;

        public RegisterEventBusConfiguration()
        {

        }

        public void ConfigureRabbitMQ(RabbitMQConfiguration rabbitMQConfiguration)
        {
            _rabbitMQConfiguration = rabbitMQConfiguration;
        }

        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            if (_rabbitMQConfiguration != null)
            {
                services.RegisterRabbitMqEventBus(_rabbitMQConfiguration);
            }
            else
            {
                services.RegisterInMemoryEventBus();
            }
        }
    }
}