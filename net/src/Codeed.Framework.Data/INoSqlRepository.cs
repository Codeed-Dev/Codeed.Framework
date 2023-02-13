using Codeed.Framework.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Codeed.Framework.Data
{
    public interface INoSqlRepository<T>
        where T : INoSqlDocument
    {
        Task<T> GetAsync(string uid, CancellationToken cancellationToken);

        Task Add(T model, CancellationToken cancellationToken);

        Task Update(string id, T model, CancellationToken cancellationToken);

        Task AddOrUpdate(string id, T model, CancellationToken cancellationToken);

        Task Delete(string id, CancellationToken cancellationToken);
    }
}
