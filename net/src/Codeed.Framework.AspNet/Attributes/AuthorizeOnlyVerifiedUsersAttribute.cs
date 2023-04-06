using Codeed.Framework.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Codeed.Framework.AspNet.Attributes
{
    public class AuthorizeOnlyVerifiedUsersAttribute : TypeFilterAttribute
    {
        public AuthorizeOnlyVerifiedUsersAttribute() : base(typeof(AuthorizeOnlyVerifiedUsersRoleFilter))
        {
        }
    }

    public class AuthorizeOnlyVerifiedUsersRoleFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity is null || !user.Identity.IsAuthenticated || !user.EmailIsVerified())
            {
                var result = new Result().Add("Unauthorized");
                context.Result = new ObjectResult(result)
                {
                    StatusCode = 401
                };
            }
        }
    }
}
