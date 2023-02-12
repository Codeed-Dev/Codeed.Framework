using System;
using System.Threading;

namespace Codeed.Framework.Concurrency
{
    public interface ISemaphore : IDisposable
    {
        void Wait(TimeSpan timeout, CancellationToken cancellationToken);
    }
}
