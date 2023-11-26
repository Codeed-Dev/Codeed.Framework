namespace Codeed.Framework.Concurrency
{
    public interface ITenantLocker
    {
        Task<IDisposable> AcquireLockAsync(string name, TimeSpan timeout, CancellationToken cancellationToken);
    }
}