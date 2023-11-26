using Medallion.Threading;

namespace Codeed.Framework.Concurrency
{
    public class DistribuitedLocker : ILocker
    {
        private IDistributedLockProvider _distributedLockProvider;

        public DistribuitedLocker(IDistributedLockProvider distributedLockProvider)
        {
            _distributedLockProvider = distributedLockProvider;
        }

        public async Task<IDisposable> AcquireLockAsync(string name, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var @lock = _distributedLockProvider.CreateLock(name);
            return await @lock.AcquireAsync(timeout, cancellationToken);
        }
    }
}
