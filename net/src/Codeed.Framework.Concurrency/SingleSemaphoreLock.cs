using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Codeed.Framework.Concurrency
{
    public class SingleSemaphoreLock : IDisposable
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();
        private readonly string _name;
        private readonly SemaphoreSlim _semaphore;

        public SingleSemaphoreLock(string name)
        {
            _name = name;
            _semaphore = _semaphores.GetOrAdd(name, (_) => new SemaphoreSlim(1, 1));
        }

        public Task Wait(TimeSpan timeout, CancellationToken cancellationToken)
        {
            return _semaphore.WaitAsync(timeout, cancellationToken);
        }

        public void Dispose()
        {
            _semaphores.Remove(_name, out var semaphore);
            _semaphore.Release();
        }
    }
}
