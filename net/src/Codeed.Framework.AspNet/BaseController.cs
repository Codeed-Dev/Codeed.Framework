using Codeed.Framework.AspNet.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codeed.Framework.AspNet
{
    [Authorize]
    [AuthorizeOnlyVerifiedUsers]
    public class BaseController : UnsafeController
    {
    }
}
