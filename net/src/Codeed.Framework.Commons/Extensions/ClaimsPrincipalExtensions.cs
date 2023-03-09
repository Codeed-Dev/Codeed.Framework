using System.Linq;
using System.Security.Claims;

namespace System
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "uid") ??
                        claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "user_id") ??
                        claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return claim == null ? string.Empty : claim.Value;
        }

        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "email") ??
                        claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            return claim == null ? string.Empty : claim.Value;
        }

        public static string GetName(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "name") ??
                        claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            var name = claim == null ? string.Empty : claim.Value;

            if (string.IsNullOrEmpty(name))
            {
                name = claimsPrincipal.GetEmail().Split("@")[0];
            }

            return name;
        }

        public static bool EmailIsVerified(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "email_verified");
            return claim == null ? true : claim.Value == "true";
        }
    }
}
