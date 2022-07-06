using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace Codeed.Framework.Data
{
    public interface IUnitOfWork
    {
        Transaction BeginTransaction();

        void EndTransaction(Transaction transaction);

        Task<bool> Commit(CancellationToken cancellationToken);

        Task<bool> Commit(Transaction transaction, CancellationToken cancellationToken);
    }
}
