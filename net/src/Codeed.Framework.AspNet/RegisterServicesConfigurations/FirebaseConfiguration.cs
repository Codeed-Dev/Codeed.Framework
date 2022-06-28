using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Codeed.Framework.AspNet.RegisterServicesConfigurations
{
    public static class RegisterCodeedFrameworkFirebaseAuthenticationOptionsExtensions
    {
        public static void ConfigureFirebaseAuthentication(this RegisterCodeedFrameworkOptions codeedOptions, string firebaseProjectId)
        {
            codeedOptions.ConfigureFirebaseAuthentication(firebaseProjectId, null);
        }

        public static void ConfigureFirebaseAuthentication(
            this RegisterCodeedFrameworkOptions codeedOptions, 
            string firebaseProjectId, 
            Action<RegisterCodeedFrameworkFirebaseAuthenticationOptions> configure)
        {
            var options = new RegisterCodeedFrameworkFirebaseAuthenticationOptions(firebaseProjectId);
            if (configure != null)
            {
                configure(options);
            }

            codeedOptions.AddServiceConfiguration(options);
        }
    }

    public class RegisterCodeedFrameworkFirebaseAuthenticationOptions : ICodeedServiceConfiguration
    {
        internal RegisterCodeedFrameworkFirebaseAuthenticationOptions(string firebaseProjectId)
        {
            FirebaseProjectId = firebaseProjectId;
        }

        public string FirebaseProjectId { get; }

        private Action<AuthenticationBuilder> _authenticationBuilder;

        public void ConfigureCustomsAuthentications(Action<AuthenticationBuilder> authenticationBuilder)
        {
            _authenticationBuilder = authenticationBuilder;
        }

        public void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var authBuilder = services.AddAuthentication(authOptions =>
                                {
                                    authOptions.DefaultAuthenticateScheme = "Firebase";
                                    authOptions.DefaultChallengeScheme = "Firebase";
                                })
                                .AddJwtBearer("Firebase", options =>
                                {
                                    options.Authority = $"https://securetoken.google.com/{FirebaseProjectId}";
                                    options.TokenValidationParameters = new TokenValidationParameters
                                    {
                                        ValidIssuer = $"https://securetoken.google.com/{FirebaseProjectId}",
                                        ValidateIssuer = true,
                                        ValidateAudience = true,
                                        ValidAudience = FirebaseProjectId,
                                        ValidateLifetime = true
                                    };
                                });

            if (_authenticationBuilder != null)
            {
                _authenticationBuilder(authBuilder);
            }
        }
    }
}