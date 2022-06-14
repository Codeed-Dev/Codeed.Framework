using Codeed.Framework.Data;
using Codeed.Framework.Identity.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Sample.Identity
{
    public class IdentityDbContext : BaseDbContext
    {
        public IdentityDbContext(DbContextOptions options, IMediator mediator) : base(options, mediator)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}