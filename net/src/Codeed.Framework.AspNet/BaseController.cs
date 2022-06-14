using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codeed.Framework.AspNet
{
    [Authorize]
    [Route("api/[controller]")]
    public class BaseController : UnsafeController
    {
    }
}
