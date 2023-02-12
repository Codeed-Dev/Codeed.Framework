using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Codeed.Framework.Concurrency
{
    public class SingleSemaphore : ISemaphore
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();

        private readonly SemaphoreSlim _semaphore;

        public SingleSemaphore(string name)
        {
            _semaphore = _semaphores.GetOrAdd(name, (_) => new SemaphoreSlim(1, 1));
        }

        public void Dispose()
        {
            _semaphore.Release();
        }

        public void Wait(TimeSpan timeout, CancellationToken cancellationToken)
        {
            _semaphore.Wait(timeout, cancellationToken);
        }
    }
}
