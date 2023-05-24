using Codeed.Framework.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public static class RegisterCodeedFrameworkDomainEventsPublisherConfigurationExtensions
    {
        public static void ConfigureDomainEventsPublisher(this RegisterCodeedFrameworkOptions codeedOptions)
        {
            var options = new DomainEventsPublisherConfiguration();
            codeedOptions.AddServiceConfiguration(options);
        }
    }

    public class DomainEventsPublisherConfiguration : ICodeedServiceConfiguration
    {
        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddScoped<IDomainEventsPublisher, EventBusDomainEventsPublisher>();
        }
    }
}