using Codeed.Framework.AspNet.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sample.Identity.Repositories;

namespace Sample.Identity
{
    public class IdentityContext : IContext
    {
        public string Name => "Identity";

        public void RegisterServices(IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptionsBuilder)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddDbContext<IdentityDbContext>(dbContextOptionsBuilder);
        }
    }
}
