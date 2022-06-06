using Microsoft.EntityFrameworkCore;
using System.Linq;
using Codeed.Framework.Domain;

namespace Codeed.Framework.Data
{
    public class BaseRepository<T> : IRepository<T> where T : Entity, IAggregateRoot
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

        public virtual IQueryable<T> QueryAll()
        {
            return DbSet;
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
