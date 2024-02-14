using Codeed.Framework.AspNet.RegisterServicesConfigurations.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public static class RegisterAzureServiceConfigurationExtensions
    {
        public static void ConfigureAzureServices(this RegisterCodeedFrameworkOptions codeedOptions, IConfiguration configuration)
        {
            codeedOptions.AddServiceConfiguration(new AzureServiceConfiguration(configuration));
        }
    }

    public class AzureServiceConfiguration : ICodeedServiceConfiguration
    {
        private readonly IConfiguration _configuration;

        public AzureServiceConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var azureConfiguration = _configuration.GetSection("Azure").Get<AzureConfiguration>();
            services.Configure<AzureConfiguration>(_configuration.GetSection("Azure"));

            if (!string.IsNullOrEmpty(azureConfiguration.ApplicationInsightsConnectionKey))
            {
                services.AddApplicationInsightsTelemetry((opt) =>
                {
                    opt.ConnectionString = azureConfiguration.ApplicationInsightsConnectionKey;
                });

                services.AddLogging(builder =>
                {
                    builder.AddApplicationInsights();
                });
            }
        }
    }
}
