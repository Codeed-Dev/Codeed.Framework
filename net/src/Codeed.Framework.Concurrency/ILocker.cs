using AsyncKeyedLock;

namespace Codeed.Framework.Concurrency
{
    public interface ILocker
    {
        ValueTask<AsyncKeyedLockTimeoutReleaser<string>> CreateAndWaitSemaphore(string name, TimeSpan timeout, CancellationToken cancellationToken);
    }
}