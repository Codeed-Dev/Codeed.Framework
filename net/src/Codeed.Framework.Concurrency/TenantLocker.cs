using AsyncKeyedLock;
using Codeed.Framework.Tenant;
using System.Runtime.CompilerServices;

namespace Codeed.Framework.Concurrency
{
    public class TenantLocker : ITenantLocker
    {
        private readonly ITenantService _tenantService;
        private readonly AsyncKeyedLocker<string> _asyncKeyedLocker;

        public TenantLocker(ITenantService tenantService, AsyncKeyedLocker<string> asyncKeyedLocker)
        {
            _tenantService = tenantService;
            _asyncKeyedLocker = asyncKeyedLocker;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueTask<AsyncKeyedLockTimeoutReleaser<string>> CreateAndWaitSemaphore(string name, TimeSpan timeout, CancellationToken cancellationToken)
        {
            return _asyncKeyedLocker.LockAsync($"{_tenantService.Tenant}-{name}", timeout, cancellationToken);
        }
    }
}