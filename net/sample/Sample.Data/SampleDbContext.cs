using Codeed.Framework.Data;
using Codeed.Framework.Tenant;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sample.Domain;

namespace Sample.Data
{
    public class SampleDbContext : BaseDbContext
    {
        public SampleDbContext(DbContextOptions options, IMediator mediator, ITenantService tenantService) : base(options, mediator, tenantService)
        {
        }
    }
}