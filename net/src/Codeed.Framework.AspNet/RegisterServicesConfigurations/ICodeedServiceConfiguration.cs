using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public interface ICodeedServiceConfiguration
    {
        void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies);
    }
}
