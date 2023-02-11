using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public static class RegisterCodeedFrameworkEventBusConfigurationExtensions
    {
        public static void ConfigureFirebaseAuthentication(this RegisterCodeedFrameworkOptions codeedOptions, string firebaseProjectId)
        {
            codeedOptions.ConfigureEventBus(firebaseProjectId, null);
        }

        public static void ConfigureEventBus(
            this RegisterCodeedFrameworkOptions codeedOptions, 
            string firebaseProjectId, 
            Action<RegisterCodeedFrameworkFirebaseAuthenticationOptions> configure)
        {
            var options = new RegisterCodeedFrameworkFirebaseAuthenticationOptions(firebaseProjectId);
            if (configure != null)
            {
                configure(options);
            }

            codeedOptions.AddServiceConfiguration(options);
        }
    }

    public class RegisterCodeedFrameworkEventBusConfiguration : ICodeedServiceConfiguration
    {
        public RegisterCodeedFrameworkEventBusConfiguration()
        {

        }

        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            
        }
    }
}