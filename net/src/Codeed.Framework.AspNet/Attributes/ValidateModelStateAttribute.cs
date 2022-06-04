using Codeed.Framework.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Codeed.Framework.AspNet.Attributes
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = new Result();

                foreach (string errorMessage in context.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage))
                {
                    result.Add(errorMessage);
                }

                context.Result = new OkObjectResult(result);
            }

            base.OnActionExecuting(context);
        }
    }
}
