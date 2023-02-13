using AsyncKeyedLock;
using System.Runtime.CompilerServices;

namespace Codeed.Framework.Concurrency
{
    public class Locker : ILocker
    {
        private readonly AsyncKeyedLocker<string> _asyncKeyedLocker;

        public Locker(AsyncKeyedLocker<string> asyncKeyedLocker)
        {
            _asyncKeyedLocker = asyncKeyedLocker;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueTask<AsyncKeyedLockTimeoutReleaser<string>> CreateAndWaitSemaphore(string name, TimeSpan timeout, CancellationToken cancellationToken)
        {
            return _asyncKeyedLocker.LockAsync(name, timeout, cancellationToken);
        }
    }
}