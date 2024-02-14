namespace Codeed.Framework.Tenant
{
    public interface ITenantScopeFactory
    {
        TenantScope CreateTransaction(string tenant);
    }
}
