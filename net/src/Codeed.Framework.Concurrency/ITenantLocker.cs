using System;
using System.Threading;

namespace Codeed.Framework.Concurrency
{
    public interface ITenantLocker
    {
        ISemaphore CreateAndWaitSemaphore(string name, TimeSpan timeout, CancellationToken cancellationToken);
    }
}