using Codeed.Framework.Tenant;
using System.Security.Claims;

namespace Sample.Web
{
    public class TenantService : UserTenantService
    {
        public TenantService(ClaimsPrincipal user) : base(user)
        {
        }

        public override IEnumerable<string> GetPermissions()
        {
            yield return "standard";
            yield return "enterprise";
        }
    }
}
