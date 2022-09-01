using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Codeed.Framework.AspNet.Context
{
    public interface IContext
    {
        string Name { get; }

        void RegisterServices(IServiceCollection services, IConfiguration configuration, Action<DbContextOptionsBuilder> dbContextOptionsBuilder);
    }
}
