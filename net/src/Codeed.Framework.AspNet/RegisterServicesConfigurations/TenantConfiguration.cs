using Codeed.Framework.Tenant;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public static class RegisterCodeedFrameworkTenantExtensions
    {
        public static void ConfigureTenant<TTenantService>(this RegisterCodeedFrameworkOptions codeedOptions) 
            where TTenantService : class, ITenantService
        {
            codeedOptions.AddServiceConfiguration(new RegisterCodeedFrameworkTenantOptions<TTenantService>());
        }
    }

    public class RegisterCodeedFrameworkTenantOptions<TTenantService> : ICodeedServiceConfiguration
        where TTenantService : class, ITenantService
    {
        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddScoped<ITenantService, TTenantService>();
            services.AddScoped<ITenantScopeFactory, TenantScopeFactory>();
        }
    }
}
