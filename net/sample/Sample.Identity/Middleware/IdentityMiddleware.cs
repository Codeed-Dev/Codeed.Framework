using Microsoft.AspNetCore.Http;
using Sample.Identity.Services;

namespace Sample.Identity.Middleware
{
    public class IdentityMiddleware
    {
        private readonly RequestDelegate _next;

        public IdentityMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext, GetCurrentUser getCurrentUser, AddUser addUser)
        {
            var cancellationToken = httpContext?.RequestAborted ?? CancellationToken.None;
            if (httpContext.User != null && httpContext.User.Identity.IsAuthenticated)
            {
                var user = await getCurrentUser.ExecuteAsync(cancellationToken);

                if (user == null)
                {
                    user = new UserDto()
                    {
                        Email = httpContext.User.GetEmail()
                    };
                    user = await addUser.ExecuteAsync(user, cancellationToken);
                }
            }

            await _next(httpContext);
        }
    }
}
