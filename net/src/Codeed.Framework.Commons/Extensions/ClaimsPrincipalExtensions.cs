using System.Linq;
using System.Security.Claims;

namespace System
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "uid") ??
                claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "user_id");
            return claim == null ? string.Empty : claim.Value;
        }

        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "email");
            return claim == null ? string.Empty : claim.Value;
        }
    }
}
