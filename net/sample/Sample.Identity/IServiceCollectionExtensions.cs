using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sample.Identity.Repositories;

namespace Sample.Identity
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services, Action<DbContextOptionsBuilder> contextOptionsBuilder)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddDbContext<IdentityDbContext>(contextOptionsBuilder);

            return services;
        }
    }
}
