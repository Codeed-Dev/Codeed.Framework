namespace Codeed.Framework.Concurrency
{
    public class InMemoryLocker : ILocker
    {
        public async Task<IDisposable> AcquireLockAsync(string name, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var semaphore = new SingleSemaphoreLock(name);
            await semaphore.Wait(timeout, cancellationToken);
            return semaphore;
        }
    }
}