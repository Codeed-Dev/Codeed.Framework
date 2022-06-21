using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Codeed.Framework.AspNet
{
    public class RegisterCoddedFrameworkSwaggerOptions
    {
        internal RegisterCoddedFrameworkSwaggerOptions()
        {
            Version = "v1";
            JwtAuthorizationDescription = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                          Enter 'Bearer' [space] and then your token in the text input below.
                          \r\n\r\nExample: 'Bearer 12345abcdef'";
        }
        
        public string Title { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }

        public string TermsOfService { get; set; }

        public string JwtAuthorizationDescription { get; set; }

        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc(Version, new OpenApiInfo
                {
                    Version = Version,
                    Title = Title,
                    Description = Description,
                });

                foreach (var assembly in assemblies)
                {
                    var xmlFile = $"{assembly.GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                        c.IncludeXmlComments(xmlPath);
                }
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = this.JwtAuthorizationDescription,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }
    }
}