using Microsoft.Extensions.DependencyInjection;
using System;
using Codeed.Framework.Services;

namespace Codeed.Framework.AspNet
{
    public abstract class ServiceController : BaseController
    {
        private IServiceProvider _serviceProvider;

        public ServiceController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Resolve<T>() where T : IServiceCore
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
