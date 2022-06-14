using Codeed.Framework.Environment.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Codeed.Framework.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Codeed.Framework.AspNet.Context;
using Microsoft.EntityFrameworkCore;

namespace Codeed.Framework.AspNet
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterCoddedFrameworkDependencies(this IServiceCollection services, string name, string assemblyPattern, Action<RegisterCoddedFrameworkOptions> configure)
        {
            var options = new RegisterCoddedFrameworkOptions(name, assemblyPattern);
            configure(options);

            if (options.AuthenticationService != null)
                options.AuthenticationService.RegisterServices(services);

            if (options.SwaggerOptions != null)
                options.SwaggerOptions.RegisterServices(services);

            services.GetAllProjectsAssemblies(options.AssemblyPattern, (assemblies) =>
            {
                services.AddMediatR(assemblies.ToArray());
                services.RegisterServicesFromAssemblies(assemblies);
                services.AddAutoMapper(assemblies.ToArray());
                services.RegisterEnvironment();
                services.RegisterContexts(assemblies, options.DbContextOptionsBuilder);

                services.RegisterCors();
                services.RegisterApiControllers(assemblies);
            });


            return services;
        }

        public static IServiceCollection RegisterContexts(this IServiceCollection services, IEnumerable<Assembly> assemblies, Action<DbContextOptionsBuilder> dbContextOptionsBuilder)
        {
            foreach (var assembly in assemblies)
            {
                services.RegisterContexts(assembly, dbContextOptionsBuilder);
            }
            return services;
        }

        public static IServiceCollection RegisterContexts(this IServiceCollection services, Assembly assembly, Action<DbContextOptionsBuilder> dbContextOptionsBuilder)
        {
            var contexts = assembly.GetTypes().Where(t => typeof(IContext).IsAssignableFrom(t) && !t.IsAbstract);
            foreach (var contextType in contexts)
            {
                var context = (IContext)Activator.CreateInstance(contextType);
                context.RegisterServices(services, dbContextOptionsBuilder);
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
            };

            mvcBuilder.AddJsonOptions(opts =>
            {
                var enumConverter = new JsonStringEnumConverter();
                opts.JsonSerializerOptions.Converters.Add(enumConverter);
            }).AddOData(opt => opt.Filter()
                                  .Expand()
                                  .Select()
                                  .OrderBy()
                                  .SetMaxTop(100));

            services.AddHttpContextAccessor();
            services.AddTransient(s => s.GetService<IHttpContextAccessor>().HttpContext.User);

            return services;
        }

        public static IServiceCollection ConfigureFirebaseAuthentication(this IServiceCollection services, string firebaseProjectId)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidAudience = firebaseProjectId,
                            ValidateLifetime = true
                        };
                    });

            return services;
        }
    }
}
