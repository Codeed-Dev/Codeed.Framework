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

        protected BaseDbContext(DbContextOptions<T> options, IDomainEventsPublisher domainEventsPublisher, ITenantService tenantService) : base(options, domainEventsPublisher, tenantService)
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

            if (currentTenantIdMemberInfo is null)
            {
                throw new NullReferenceException($"{nameof(currentTenantIdMemberInfo)} is null. Check if Tenant exists in {this.GetType().Name}");
            }

            foreach (var entityType in modelBuilder.GetEntities<IEntityWithTenant>())
            {
                var tenantProperty = entityType.FindProperty(nameof(IEntityWithTenant.Tenant));
                if (tenantProperty is null || tenantProperty.PropertyInfo is null)
                    continue;

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
                if (tenantProperty is null)
                {
                    throw new ArgumentNullException($"{nameof(tenantProperty)} is null in a {nameof(IEntityWithTenant)} entity: {entity.GetType().FullName}");
                }
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
