using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scaffolding.Core.Application.Environment;
using System.Reflection;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public static class RegisterCodeedFrameworkSendGridConfigurationExtensions
    {
        public static void ConfigureSendGrid(this RegisterCodeedFrameworkOptions codeedOptions, IConfiguration configuration)
        {
            var options = new SendGridEmailServiceConfiguration(configuration);
            codeedOptions.AddServiceConfiguration(options);
        }
    }

    public class SendGridEmailServiceConfiguration : ICodeedServiceConfiguration
    {
        private readonly IConfiguration _configuration;

        public SendGridEmailServiceConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.Configure<SendGridConfiguration>(_configuration.GetSection("SendGrid"));
        }
    }
}
