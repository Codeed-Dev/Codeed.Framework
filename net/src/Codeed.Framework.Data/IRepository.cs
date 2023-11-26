using Codeed.Framework.Domain;
using Codeed.Framework.Specification;

namespace Codeed.Framework.Data
{
    public interface IRepository<T> : IDisposable where T : Entity, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        Task Reload(T entity, CancellationToken cancellationToken);

        IQueryable<T> QueryAll();

        IQueryable<T> QueryAll(ISpecification<T> spec);

        IQueryable<T> QueryById(Guid id);

        Task<T?> GetById(Guid id, CancellationToken cancellationToken);

        IQueryable<T> IncludeAll(IQueryable<T> queryable);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(IQueryable<T> entities);
    }
}
