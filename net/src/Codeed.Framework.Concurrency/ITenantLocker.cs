using AsyncKeyedLock;
using System;
using System.Threading;

namespace Codeed.Framework.Concurrency
{
    public interface ITenantLocker
    {
        ValueTask<AsyncKeyedLockTimeoutReleaser<string>> CreateAndWaitSemaphore(string name, TimeSpan timeout, CancellationToken cancellationToken);
    }
}