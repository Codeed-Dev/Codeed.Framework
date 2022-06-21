using Codeed.Framework.AspNet.Context;
using Codeed.Framework.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sample.Data;
using Sample.Domain;
using Sample.Domain.Repositories;

namespace Sample.CrossCutting
{
    public class SampleContext : IContext
    {
        public string Name => "Sample";

        public void RegisterServices(IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptionsBuilder)
        {
            services.RegisterRepository<ICustomerRepository, CustomerRepository, Customer>();
            services.AddDbContext<SampleDbContext>(dbContextOptionsBuilder);
        }
    }
}
