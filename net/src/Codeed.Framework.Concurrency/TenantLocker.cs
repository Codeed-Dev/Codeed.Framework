using Codeed.Framework.Tenant;
using System;
using System.Threading;

namespace Codeed.Framework.Concurrency
{
    public class TenantLocker : ITenantLocker
    {
        private readonly ITenantService _tenantService;
        private readonly ILocker _locker;

        public TenantLocker(ITenantService tenantService, ILocker locker)
        {
            _tenantService = tenantService;
            _locker = locker;
        }

        public Task<IDisposable> AcquireLock(string name, TimeSpan timeout, CancellationToken cancellationToken)
        {
            return _locker.AcquireLockAsync($"{_tenantService.Tenant}-{name}", timeout, cancellationToken);
        }
    }
}