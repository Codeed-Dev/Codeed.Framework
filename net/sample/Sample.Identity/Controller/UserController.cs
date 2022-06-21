using Codeed.Framework.AspNet;
using Microsoft.AspNetCore.Mvc;
using Sample.Identity.Services;
using System.Security.Claims;

namespace Sample.Identity.Controller
{
    public class UserController : ServiceController
    {
        private readonly ClaimsPrincipal _principal;

        public UserController(IServiceProvider serviceProvider, ClaimsPrincipal principal) : base(serviceProvider)
        {
            _principal = principal;
        }

        [HttpGet]
        public async Task<IActionResult> CurrentUser(CancellationToken cancellationToken)
        {
            var user = await Resolve<GetCurrentUser>().ExecuteAsync(cancellationToken);
            if (user == null)
            {
                user = new UserDto()
                {
                    Email = _principal.GetEmail()
                };
                user = await Resolve<AddUser>().ExecuteAsync(user, cancellationToken);
            }

            return Ok(user);
        }
    }
}
