using Codeed.Framework.Data;
using Codeed.Framework.EventBus;
using Codeed.Framework.Tenant;
using Microsoft.EntityFrameworkCore;

namespace Sample.Data
{
    public class SampleDbContext : BaseDbContext<SampleDbContext>
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options, IEventBus eventBus, ITenantService tenantService) : base(options, eventBus, tenantService)
        {
        }
    }
}