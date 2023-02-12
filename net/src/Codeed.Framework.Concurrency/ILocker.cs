using System;
using System.Threading;

namespace Codeed.Framework.Concurrency
{
    public interface ILocker
    {
        ISemaphore CreateAndWaitSemaphore(string name, TimeSpan timeout, CancellationToken cancellationToken);
    }
}