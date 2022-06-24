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

        public UserTenantService(ClaimsPrincipal user)
        {
            _user = user;
        }

        public string Tenant => _user.GetUserId();

        public bool Authorize(object permission)
        {
            return GetPermissions().Contains(permission);
        }

        public IEnumerable<string> GetTenants()
        {
            yield return _user.GetUserId();
        }

        public abstract IEnumerable<object> GetPermissions();

        public string GetRootUserUid() => _user.GetUserId();
    }
}
