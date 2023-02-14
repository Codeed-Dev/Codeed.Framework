using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Codeed.Framework.Domain;
using System.Linq.Expressions;
using Codeed.Framework.Tenant;
using Codeed.Framework.Data.Extensions;
using Codeed.Framework.Domain.Exceptions;
using Codeed.Framework.EventBus;

namespace Codeed.Framework.Data
{
    public abstract class BaseDbContext<T> : UnsafeBaseDbContext<T>
        where T : BaseDbContext<T>
    {
        private readonly ITenantService _tenantService;

        protected BaseDbContext(DbContextOptions<T> options, IEventBus eventBus, ITenantService tenantService) : base(options, eventBus, tenantService)
        {
            _tenantService = tenantService;
        }

        public string Tenant => _tenantService.Tenant;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyTenantFilter(modelBuilder);
        }


        private void ApplyTenantFilter(ModelBuilder modelBuilder)
        {
            var currentTenantIdMemberInfo = GetType().GetMember(nameof(Tenant)).FirstOrDefault();

            foreach (var entityType in modelBuilder.GetEntities<IEntityWithTenant>())
            {
                var tenantProperty = entityType.FindProperty(nameof(IEntityWithTenant.Tenant));
                var parameter = Expression.Parameter(entityType.ClrType, "tenant");
                var filter = Expression.Lambda(Expression.Equal(
                    Expression.Property(parameter, tenantProperty.PropertyInfo), Expression.MakeMemberAccess(Expression.Constant(this), currentTenantIdMemberInfo)), parameter);
                entityType.SetQueryFilter(filter);
            }
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            ValidateTenant();

            return base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void ValidateTenant()
        {
            var entitiesWithTenant = ChangeTracker.Entries<IEntityWithTenant>()
                                                  .Where(e => e.State == EntityState.Added)
                                                  .Select(e => e.Entity)
                                                  .Where(e => string.IsNullOrEmpty(e.Tenant));


            foreach (var entity in entitiesWithTenant)
            {
                var tenantProperty = entity.GetType().GetProperty(nameof(IEntityWithTenant.Tenant));
                tenantProperty.SetValue(entity, _tenantService.Tenant, null);
            }

            var existsEntitiesOthersTenant = ChangeTracker.Entries<IEntityWithTenant>()
                                                          .Where(e => e.Entity.Tenant != _tenantService.Tenant)
                                                          .Any();

            if (existsEntitiesOthersTenant)
            {
                throw new TenantException("Cannot change entries from other tenant");
            }
        }
    }
}
