namespace Codeed.Framework.Tenant
{
    public class TenantScopeFactory : ITenantScopeFactory
    {
        private readonly ITenantService _tenantService;

        public TenantScopeFactory(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        public TenantScope CreateTransaction(string tenant)
        {
            return new TenantScope(_tenantService, tenant);
        }
    }
}
