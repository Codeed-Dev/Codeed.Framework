using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Filters;

namespace Codeed.Framework.AspNet.Serilog
{
    public static class SerilogConfigurationExtensions
    {
        public static void AddSerilog(this IHostBuilder hostBuilder)
        {

        }

        public static void AddSerilogApi(this IConfiguration configuration, string applicationName)
        {
            Log.Logger = new LoggerConfiguration()
            //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithCorrelationId()
            .Enrich.WithProperty("ApplicationName", $"API Serilog - {applicationName}")
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
            .WriteTo.Async(wt => wt.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Properties:j}{NewLine}{Exception}"))
            .CreateLogger();
        }
    }
}
