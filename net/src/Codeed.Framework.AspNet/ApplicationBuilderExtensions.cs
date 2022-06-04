using Codeed.Framework.Commons;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Codeed.Framework.AspNet
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseResultExceptionHandler(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseExceptionHandler(ConfigureExceptionHandler);
            return applicationBuilder;
        }

        private static void ConfigureExceptionHandler(IApplicationBuilder errorApp)
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var logger = errorApp.ApplicationServices.GetRequiredService<ILogger<IApplicationBuilder>>();

                var error = context.Features.Get<IExceptionHandlerFeature>();
                if (error != null)
                {
                    logger.LogError(error.Error, $"Falha na requisição {error.Path}");
                    var ex = error.Error;

                    var result = new Result();
                    result.Add(ex);

                    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                }
            });
        }
    }
}
