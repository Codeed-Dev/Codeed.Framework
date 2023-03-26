namespace Codeed.Framework.Tenant
{
    public class FixedTenantService : ITenantService
    {
        public string Tenant => "-1";

        public bool Authorize(object permission)
        {
            return true;
        }

        public IEnumerable<object> GetPermissions()
        {
            return Enumerable.Empty<object>();
        }

        public void SetTenant(string tenant)
        {
            
        }
    }
}
