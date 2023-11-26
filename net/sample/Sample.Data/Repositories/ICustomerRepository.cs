using Codeed.Framework.Data;
using Microsoft.EntityFrameworkCore;
using Sample.Data;

namespace Sample.Domain.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(SampleDbContext context) : base(context, context)
        {
        }

        public override IQueryable<Customer> IncludeAll(IQueryable<Customer> queryable)
        {
            return queryable;
        }
    }
}
