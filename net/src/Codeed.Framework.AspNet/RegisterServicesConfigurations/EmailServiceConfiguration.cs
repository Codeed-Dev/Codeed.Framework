using Codeed.Framework.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scaffolding.Core.Application.Email;
using System.Reflection;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public static class RegisterEmailServiceConfigurationExtensions
    {
        public static void ConfigureEmailService(this RegisterCodeedFrameworkOptions codeedOptions)
        {
            codeedOptions.AddServiceConfiguration(new EmailServiceConfiguration());
        }
    }

    public class EmailServiceConfiguration : ICodeedServiceConfiguration
    {
        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddScoped<IEmailService, SendGridEmailService>();
        }
    }
}
