using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;

namespace Codeed.Framework.AspNet.Serilog
{
    public static class SerilogConfigurationExtensions
    {
        public static void AddSerilogApi(this IConfiguration configuration, string applicationName)
        {
            configuration.AddSerilogApi(applicationName, null);
        }

        public static void AddSerilogApi(this IConfiguration configuration, string applicationName, Func<LoggerConfiguration, LoggerConfiguration>? configureLogger)
        {
            var loggerConfiguration = new LoggerConfiguration();
            ConfigureDefaultLogger(loggerConfiguration, applicationName);

            if (configureLogger is not null)
            {
                loggerConfiguration = configureLogger(loggerConfiguration);
            }

            Log.Logger = loggerConfiguration.CreateLogger();
        }

        public static void AddSerilog(this IHostBuilder host, string applicationName, Action<LoggerConfiguration>? configureLogger)
        {
            host.UseSerilog((context, services, loggerConfiguration) =>
            {
                var telemetryConfiguration = services.GetService<TelemetryConfiguration>();
                ConfigureDefaultLogger(loggerConfiguration, applicationName);

                if (configureLogger is not null)
                {
                    configureLogger(loggerConfiguration);
                }

                if (telemetryConfiguration is not null)
                {
                    loggerConfiguration.WriteTo.ApplicationInsights(telemetryConfiguration, new TraceTelemetryConverter(), LogEventLevel.Information);
                }
            });
        }

        private static void ConfigureDefaultLogger(LoggerConfiguration logger, string applicationName)
        {
            logger.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
                  .Enrich.FromLogContext()
                  .Enrich.WithCorrelationId()
                  .Enrich.WithProperty("ApplicationName", $"API Serilog - {applicationName}")
                  .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
                  .WriteTo.Async(wt => wt.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Properties:j}{NewLine}{Exception}"));
        }
    }
}
