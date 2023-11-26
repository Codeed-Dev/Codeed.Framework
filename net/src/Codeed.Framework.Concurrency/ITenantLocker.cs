namespace Codeed.Framework.Concurrency
{
    public interface ITenantLocker
    {
        Task<IDisposable> AcquireLock(string name, TimeSpan timeout, CancellationToken cancellationToken);
    }
}