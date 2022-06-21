using Codeed.Framework.AspNet;
using Microsoft.AspNetCore.Mvc;

namespace Sample.WebApi.Controllers
{
    public class TesteController : ServiceController
    {
        public TesteController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("working");
        }
    }
}
