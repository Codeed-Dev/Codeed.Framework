using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace Codeed.Framework.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit(CancellationToken cancellationToken);
    }
}
