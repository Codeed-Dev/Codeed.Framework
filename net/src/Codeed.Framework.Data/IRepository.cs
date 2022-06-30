using Codeed.Framework.Domain;
using System;
using System.Linq;

namespace Codeed.Framework.Data
{
    public interface IRepository<T> : IDisposable where T : Entity, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        IQueryable<T> QueryAll();

        IQueryable<T> QueryById(Guid id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(IQueryable<T> entities);
    }
}
