using Codeed.Framework.Data;
using Codeed.Framework.Data.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Authentication;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public static class RegisterCodeedFrameworkMongoDbConfigurationExtensions
    {
        public static void ConfigureMongoDb(
            this RegisterCodeedFrameworkOptions codeedOptions,
            IConfigurationSection section)
        {
            codeedOptions.ConfigureMongoDb(section, null);
        }

        public static void ConfigureMongoDb(
            this RegisterCodeedFrameworkOptions codeedOptions,
            IConfigurationSection section,
            Action<RegisterMongoDbConfiguration> configure)
        {
            var options = new RegisterMongoDbConfiguration(section);
            if (configure != null)
            {
                configure(options);
            }
            codeedOptions.AddServiceConfiguration(options);
        }
    }

    public class RegisterMongoDbConfiguration : ICodeedServiceConfiguration
    {
        private readonly IConfigurationSection _section;

        public RegisterMongoDbConfiguration(IConfigurationSection section)
        {
            _section = section;
        }

        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.Configure<MongoDbConfiguration>(_section);
            services.AddSingleton(typeof(INoSqlRepository<>), typeof(MongoDbNoSqlRepository<>));

            services.AddSingleton<IMongoClient>(s => {
                var configuration = s.GetRequiredService<IOptions<MongoDbConfiguration>>();
                string connectionString = configuration.Value.ConnectionString;
                var settings = MongoClientSettings.FromUrl(
                  new MongoUrl(connectionString)
                );
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
                return new MongoClient(settings);
            });

            services.AddSingleton<IMongoDatabase>(s =>
            {
                var configuration = s.GetRequiredService<IOptions<MongoDbConfiguration>>();
                return s.GetRequiredService<IMongoClient>().GetDatabase(configuration.Value.Database);
            });
        }
    }
}