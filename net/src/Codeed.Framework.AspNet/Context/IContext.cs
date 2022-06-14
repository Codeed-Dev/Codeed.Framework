using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Codeed.Framework.AspNet.Context
{
    public interface IContext
    {
        string Name { get; }

        void RegisterServices(IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptionsBuilder);
    }
}
