using Codeed.Framework.Tenant;
using System;
using System.Threading;

namespace Codeed.Framework.Concurrency
{
    public class Locker : ILocker
    {
        public ISemaphore CreateAndWaitSemaphore(string name, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var semaphore = new SingleSemaphore(name);
            semaphore.Wait(timeout, cancellationToken);

            return semaphore;
        }
    }
}