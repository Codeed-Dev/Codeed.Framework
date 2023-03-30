using Codeed.Framework.Commons;
using Codeed.Framework.Tenant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Codeed.Framework.Services.Http.Attributes
{
    public class TenantAuthorizeAttribute : TypeFilterAttribute
    {
        public TenantAuthorizeAttribute(object permission) : base(typeof(AuthorizationTenantFilter))
        {
            Arguments = new object[] { permission };
        }
    }

    public class AuthorizationTenantFilter : IAuthorizationFilter
    {
        private readonly object _permission;
        private readonly ITenantService _tenantService;

        public AuthorizationTenantFilter(object permission, ITenantService tenantService)
        {
            _permission = permission;
            _tenantService = tenantService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!_tenantService.Authorize(_permission))
            {
                var result = new Result().Add("You don't have permission");
                context.Result = new ObjectResult(result)
                {
                    StatusCode = 403
                };
            }
        }
    }
}
