using Microsoft.AspNetCore.Mvc;
using Codeed.Framework.AspNet.Attributes;

namespace Codeed.Framework.AspNet
{
    [ApiController]
    [ResultResponse]
    [ValidateModelState]
    public class UnsafeController : ControllerBase
    {
    }
}
