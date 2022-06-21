using Codeed.Framework.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Sample.Data
{
    public class SampleDbContext : BaseDbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options, IMediator mediator) : base(options, mediator)
        {
        }
    }
}