using Codeed.Framework.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Codeed.Framework.AspNet.Attributes
{
    public class ResultResponseAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context?.Result is ObjectResult)
            {
                var objectResult = (ObjectResult)context.Result;

                if (!(objectResult.Value is IResult))
                {
                    var status = new Result<object>().Ok(objectResult.Value);
                    objectResult.Value = status;
                }

                context.Result = new OkObjectResult(objectResult.Value);
            }

            base.OnActionExecuted(context);
        }
    }
}
