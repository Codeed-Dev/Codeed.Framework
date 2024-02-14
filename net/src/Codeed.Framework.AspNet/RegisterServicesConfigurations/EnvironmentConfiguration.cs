using Codeed.Framework.Environment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public static class RegisterEnvironmentServiceConfigurationExtensions
    {
        public static void ConfigureEnvironmentServices(this RegisterCodeedFrameworkOptions codeedOptions, IConfiguration configuration)
        {
            codeedOptions.AddServiceConfiguration(new EnvironmentServiceConfiguration(configuration));
        }
    }

    public class EnvironmentServiceConfiguration : ICodeedServiceConfiguration
    {
        private readonly IConfiguration _configuration;

        public EnvironmentServiceConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.Configure<Environment.EnvironmentConfiguration>(_configuration.GetSection("Environment"));
            services.AddSingleton<IEnvironment, Environment.Environment>();
        }
    }
}
