using Codeed.Framework.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Codeed.Framework.AspNet.Tenant
{
    public abstract class UserTenantService : ITenantService
    {
        private readonly ClaimsPrincipal _user;

        protected UserTenantService(ClaimsPrincipal user)
        {
            _user = user;
            Tenant = _user.GetUserId();
        }

        public string Tenant { get; private set; }

        public bool Authorize(object permission)
        {
            return GetPermissions().Contains(permission);
        }

        public abstract IEnumerable<object> GetPermissions();


        public void SetTenant(string tenant)
        {
            Tenant = tenant;
        }
    }
}
