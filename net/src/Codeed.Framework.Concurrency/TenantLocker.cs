using Codeed.Framework.Tenant;
using System;
using System.Threading;

namespace Codeed.Framework.Concurrency
{
    public class TenantLocker : ITenantLocker
    {
        private readonly ITenantService _tenantService;

        public TenantLocker(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        public ISemaphore CreateAndWaitSemaphore(string name, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var semaphore = new SingleSemaphore($"{_tenantService.Tenant}-{name}");
            semaphore.Wait(timeout, cancellationToken);

            return semaphore;
        }
    }
}