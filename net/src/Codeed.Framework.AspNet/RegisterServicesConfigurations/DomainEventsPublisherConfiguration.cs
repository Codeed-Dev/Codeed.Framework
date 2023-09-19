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

        public static void ConfigureDomainEventsPublisher(this RegisterCodeedFrameworkOptions codeedOptions, Action<DomainEventsPublisherConfiguration>? configure)
        {
            var options = new DomainEventsPublisherConfiguration();
            if (configure is not null)
                configure(options);

            codeedOptions.AddServiceConfiguration(options);
        }
    }

    public enum IDomainEventsPublisherTypes
    {
        Mediator,
        EventBus
    }  

    public class DomainEventsPublisherConfiguration : ICodeedServiceConfiguration
    {
        public IDomainEventsPublisherTypes PublisherType { get; set; } = IDomainEventsPublisherTypes.Mediator;

        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            if (PublisherType == IDomainEventsPublisherTypes.EventBus)
            {
                services.AddScoped<IDomainEventsPublisher, EventBusDomainEventsPublisher>();
            }
            else
            {
                services.AddScoped<IDomainEventsPublisher, MediatorDomainEventsPublisher>();
            }
        }
    }
}