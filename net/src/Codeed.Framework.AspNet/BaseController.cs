using Codeed.Framework.AspNet.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codeed.Framework.AspNet
{
    [Authorize]
    public class BaseController : UnsafeController
    {
    }
}
