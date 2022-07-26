using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Codeed.Framework.Commons;
using Codeed.Framework.Commons.Exceptions;

namespace Codeed.Framework.AspNet.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (context == null)
            {
                return StatusCode(500, new Result().Add("Internal Error"));
            }

            var ex = context.Error;
            var result = new Result();

            if (ex is IHttpException httpException)
            {
                result.AddError(ex, httpException.Code, httpException.Parameters);
                _logger.LogWarning(ex, ex.Message, context.Path);
                return StatusCode(httpException.HttpCode, result);
            }

            result.Add(ex);
            _logger.LogError(ex, $"Falha na requisição {context.Path}");
            return StatusCode(500, result);
        }
    }
}
