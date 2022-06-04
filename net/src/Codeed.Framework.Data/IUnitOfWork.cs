using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace Influencer.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit(CancellationToken cancellationToken);
    }
}
