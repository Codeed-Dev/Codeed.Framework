namespace Codeed.Framework.Tenant
{
    public class TenantScope : IDisposable
    {
        private readonly ITenantService _tenantService;
        private readonly string _currentTenant;

        public TenantScope(ITenantService tenantService, string newTenant)
        {
            _tenantService = tenantService;
            _currentTenant = tenantService.Tenant;
            _tenantService.SetTenant(newTenant);
        }

        public void Dispose()
        {
            _tenantService.SetTenant(_currentTenant);
        }
    }
}
