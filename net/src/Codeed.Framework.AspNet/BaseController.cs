using Microsoft.AspNetCore.Authorization;

namespace Codeed.Framework.AspNet
{
    [Authorize]
    public class BaseController : UnsafeController
    {
    }
}
