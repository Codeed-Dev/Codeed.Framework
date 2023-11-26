using Codeed.Framework.Environment.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Codeed.Framework.Services;
using MediatR;
using Codeed.Framework.AspNet.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Codeed.Framework.EventBus;
using Codeed.Framework.Concurrency;
using Codeed.Framework.Domain.Validations;
using Codeed.Framework.AspNet.Maps;
using System.Security.Claims;

namespace Codeed.Framework.AspNet
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterCodeedFrameworkDependencies(this IServiceCollection services, IConfiguration configuration, string name, string assemblyPattern, Action<RegisterCodeedFrameworkOptions> configure)
        {
            var options = new RegisterCodeedFrameworkOptions(name, assemblyPattern);
            configure(options);

            services.GetAllProjectsAssemblies(options.AssemblyPattern, (assemblies) =>
            {
                services.AddSingleton<IEnumerable<Assembly>>(assemblies);

                foreach (var serviceConfiguration in options.ServicesConfigurations)
                {
                    serviceConfiguration.RegisterServices(services, assemblies);
                }

                services.AddMediatR((config) => config.RegisterServicesFromAssemblies(assemblies.ToArray()));
                services.RegisterServicesFromAssemblies(assemblies);
                services.AddAutoMapper(assemblies.ToArray());
                services.AddAutoMapper(typeof(DateTimeMaps).Assembly);
                services.RegisterEnvironment();
                services.RegisterContexts(configuration, assemblies, options.DbContextOptionsBuilder);
                services.RegisterValidations(assemblies);
                services.RegisterCors();
                services.RegisterApiControllers(assemblies);
            });

            return services;
        }

        private static void RegisterValidations(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.Scan(scan =>
            {
                scan.FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo(typeof(IUpdateValidation<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();

                scan.FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo(typeof(IDeleteValidation<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();

                scan.FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo(typeof(ICreateValidation<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
        }

        public static IServiceCollection RegisterContexts(this IServiceCollection services, IConfiguration configuration, IEnumerable<Assembly> assemblies, Action<DbContextOptionsBuilder>? dbContextOptionsBuilder)
        {
            foreach (var assembly in assemblies)
            {
                services.RegisterContexts(configuration, assembly, dbContextOptionsBuilder);
            }
            return services;
        }

        public static IServiceCollection RegisterContexts(this IServiceCollection services, IConfiguration configuration, Assembly assembly, Action<DbContextOptionsBuilder>? dbContextOptionsBuilder)
        {
            var contexts = assembly.GetTypes().Where(t => typeof(IContext).IsAssignableFrom(t) && !t.IsAbstract);
            foreach (var contextType in contexts)
            {
                var context = Activator.CreateInstance(contextType) as IContext;
                if (context is not null)
                {
                    context.RegisterServices(services, configuration, dbContextOptionsBuilder);
                }
            }

            return services;
        }

        private static IServiceCollection RegisterCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "__allOrigins",
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                             .AllowAnyHeader()
                                             .AllowAnyMethod();
                                  });
            });

            return services;
        }

        private static IServiceCollection RegisterApiControllers(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            var mvcBuilder = services.AddControllers();
            foreach (var assembly in assemblies)
            {
                mvcBuilder.AddApplicationPart(assembly);
            }

            mvcBuilder.AddJsonOptions(opts =>
            {
                var enumConverter = new JsonStringEnumConverter();
                opts.JsonSerializerOptions.Converters.Add(enumConverter);
            }).AddOData(opt => opt.Filter()
                                  .Expand()
                                  .Select()
                                  .Count()
                                  .OrderBy()
                                  .SetMaxTop(100));

            services.AddHttpContextAccessor();
            services.AddTransient(s => s.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? new ClaimsPrincipal());

            return services;
        }
    }
}
