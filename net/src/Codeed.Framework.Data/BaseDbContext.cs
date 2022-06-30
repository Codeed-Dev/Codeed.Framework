using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Codeed.Framework.Domain;
using System.Linq.Expressions;
using Codeed.Framework.Tenant;
using Codeed.Framework.Data.Extensions;
using Codeed.Framework.Domain.Exceptions;

namespace Codeed.Framework.Data
{
    public abstract class BaseDbContext<T> : DbContext, IUnitOfWork
        where T : BaseDbContext<T>
    {
        private readonly IMediator _mediator;
        private readonly ITenantService _tenantService;

        protected BaseDbContext(DbContextOptions<T> options, IMediator mediator, ITenantService tenantService) : base(options)
        {
            _mediator = mediator;
            _tenantService = tenantService;
        }

        public async Task<bool> Commit(CancellationToken cancellationToken)
        {
            var success = await SaveChangesAsync(cancellationToken) > 0;

            return success;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            ApplyTenantFilter(modelBuilder);
            ConfigureEntitiesTables(modelBuilder);
        }

        private void ConfigureEntitiesTables(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.GetEntities<EntityWithoutTenant>())
            {
                modelBuilder.Entity(entityType.ClrType).Property(nameof(EntityWithoutTenant.CreatedDate)).HasColumnName("CREATED_DATE");
                modelBuilder.Entity(entityType.ClrType).Property(nameof(EntityWithoutTenant.Id)).HasColumnName("ID");
                modelBuilder.Entity(entityType.ClrType).HasIndex(nameof(EntityWithoutTenant.CreatedDate));
            }

            foreach (var entityType in modelBuilder.GetEntities<Entity>())
            {
                modelBuilder.Entity(entityType.ClrType).Property(nameof(Entity.Tenant)).HasColumnName("TENANT");
                modelBuilder.Entity(entityType.ClrType).HasIndex(nameof(Entity.Tenant));
                modelBuilder.Entity(entityType.ClrType).Property(nameof(Entity.Tenant)).IsRequired();
            }
        }

        private void ApplyTenantFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.GetEntities<IEntityWithTenant>())
            {
                var tenantProperty = entityType.FindProperty(nameof(IEntityWithTenant.Tenant));
                var parameter = Expression.Parameter(entityType.ClrType, "tenant");
                var filter = Expression.Lambda(Expression.Equal(
                    Expression.Property(parameter, tenantProperty.PropertyInfo), Expression.Constant(_tenantService.Tenant)), parameter);
                entityType.SetQueryFilter(filter);
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            ValidateTenant();

            var entitiesWithEvents = ChangeTracker.Entries<Entity>()
                                                  .Select(e => e.Entity)
                                                  .Where(e => e.Events.Any())
                                                  .ToArray();

            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            if (_mediator == null)
            {
                return result;
            }

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.ClearDomainEvents();
                foreach (var domainEvent in events)
                {
                    await _mediator.Publish(domainEvent).ConfigureAwait(false);
                }
            }

            return result;
        }

        private void ValidateTenant()
        {
            var entitiesWithoutTenant = ChangeTracker.Entries<IEntityWithTenant>()
                                                     .Where(e => e.State == EntityState.Added)
                                                     .Select(e => e.Entity)
                                                     .Where(e => string.IsNullOrEmpty(e.Tenant));


            foreach (var entity in entitiesWithoutTenant)
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

        public override int SaveChanges()
        {
            return SaveChangesAsync(CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}
