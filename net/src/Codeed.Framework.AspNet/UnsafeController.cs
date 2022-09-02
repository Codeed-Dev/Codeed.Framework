using Microsoft.AspNetCore.Mvc;
using Codeed.Framework.AspNet.Attributes;

namespace Codeed.Framework.AspNet
{
    [Route("api/[controller]")]
    [ApiController]
    [ResultResponse]
    [ValidateModelState]
    public class UnsafeController : ControllerBase
    {
    }
}
