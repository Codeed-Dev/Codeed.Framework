using Microsoft.Extensions.DependencyInjection;

namespace Codeed.Framework.AspNet
{
    internal interface ICoddedFrameworkAuthenticationService
    {
        void RegisterServices(IServiceCollection services);
    }
}