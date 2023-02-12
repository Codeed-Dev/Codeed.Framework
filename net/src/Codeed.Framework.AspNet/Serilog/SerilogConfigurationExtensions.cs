using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Filters;
using System;

namespace Codeed.Framework.AspNet.Serilog
{
    public static class SerilogConfigurationExtensions
    {
        public static void AddSerilogApi(this IConfiguration configuration, string applicationName)
        {
            configuration.AddSerilogApi(applicationName, null);
        }

        public static void AddSerilogApi(this IConfiguration configuration, string applicationName, Func<LoggerConfiguration, LoggerConfiguration> configureLogger)
        {
            var loggerConfiguration = new LoggerConfiguration()
                //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .Enrich.WithProperty("ApplicationName", $"API Serilog - {applicationName}")
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
                .WriteTo.Async(wt => wt.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Properties:j}{NewLine}{Exception}"));

            if (configureLogger != null)
            {
                loggerConfiguration = configureLogger(loggerConfiguration);
            }

            Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
}
