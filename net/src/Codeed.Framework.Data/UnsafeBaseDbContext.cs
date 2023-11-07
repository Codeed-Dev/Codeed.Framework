using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Codeed.Framework.Domain;
using Codeed.Framework.Data.Extensions;
using System.Collections.Generic;
using System;
using Codeed.Framework.EventBus;
using Codeed.Framework.Tenant;

namespace Codeed.Framework.Data
{
    public abstract class UnsafeBaseDbContext<T> : DbContext, IUnitOfWork
        where T : UnsafeBaseDbContext<T>
    {
        private readonly IDomainEventsPublisher _domainEventsPublisher;
        private readonly ITenantService _tenantService;
        private Transaction? _currentTransaction;

        protected UnsafeBaseDbContext(
            DbContextOptions<T> options,
            IDomainEventsPublisher domainEventsPublisher,
            ITenantService tenantService) : base(options)
        {
            _domainEventsPublisher = domainEventsPublisher;
            _tenantService = tenantService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
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

        public Task<bool> Commit(CancellationToken cancellationToken)
        {
            return Commit(null, cancellationToken);
        }

        public async Task<bool> Commit(Transaction? transaction, CancellationToken cancellationToken)
        {
            if (!IsCurrentTransaction(transaction))
            {
                return true;
            }

            var success = await SaveChangesAsync(cancellationToken) > 0;
            return success;
        }

        public Transaction BeginTransaction()
        {
            var transaction = new Transaction(this);

            if (_currentTransaction is null)
            {
                _currentTransaction = transaction;
            }
            
            return transaction;
        }

        public void EndTransaction(Transaction transaction)
        {
            if (IsCurrentTransaction(transaction))
            {
                _currentTransaction = null;
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var entitiesWithEvents = ChangeTracker.Entries<EntityWithoutTenant>()
                                                  .Select(e => e.Entity)
                                                  .Where(e => e.Events.Any())
                                                  .ToArray();

            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            if (_domainEventsPublisher is null)
            {
                return result;
            }

            var events = entitiesWithEvents.SelectMany(e => e.Events)
                                           .Distinct()
                                           .ToList();

            foreach (var entity in entitiesWithEvents)
            {
                entity.ClearDomainEvents();
            }

            var previousTransaction = _currentTransaction;
            try
            {
                // Limpa a transaction temporariamente pois os eventos poderão ser executados no mesmo escopo de execução
                _currentTransaction = null;
                await _domainEventsPublisher.Publish(events);
            }
            finally
            {
                _currentTransaction = previousTransaction;
            }

            return result;
        }

        private bool IsCurrentTransaction(Transaction? transaction)
        {
            return _currentTransaction is null ||
                _currentTransaction == transaction;
        }
    }
}
