using Microsoft.EntityFrameworkCore;
using System.Linq;
using Codeed.Framework.Domain;
using System;
using Codeed.Framework.Specification;

namespace Codeed.Framework.Data
{
    public abstract class BaseRepository<T> : IRepository<T> where T : Entity, IAggregateRoot
    {
        protected readonly DbContext Context;
        protected readonly DbSet<T> DbSet;

        public BaseRepository(DbContext context, IUnitOfWork unifOfWork)
        {
            Context = context;
            DbSet = Context.Set<T>();
            UnitOfWork = unifOfWork;
        }

        public IUnitOfWork UnitOfWork { get; private set; }

        public virtual Task Reload(T entity, CancellationToken cancellationToken)
        {
            return Context.Entry<T>(entity).ReloadAsync();
        }

        public virtual IQueryable<T> QueryAll()
        {
            return DbSet;
        }

        public virtual IQueryable<T> QueryById(Guid id)
        {
            return QueryAll().Where(a => a.Id == id);
        }

        public virtual IQueryable<T> QueryAll(ISpecification<T> spec)
        {
            return QueryAll().Where(r => spec.IsSatisfiedBy(r));
        }

        public abstract IQueryable<T> IncludeAll(IQueryable<T> queryable);

        public virtual Task<T?> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = QueryAll().Where(a => a.Id == id);
            query = IncludeAll(query);
            return query.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void Delete(IQueryable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
