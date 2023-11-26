using System;
using System.Threading;

namespace Codeed.Framework.Concurrency
{
    public interface ILocker
    {
        Task<IDisposable> AcquireLockAsync(string name, TimeSpan timeout, CancellationToken cancellationToken);
    }
}