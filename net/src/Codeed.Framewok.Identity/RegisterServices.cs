using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Codeed.Framewok.Identity
{
    public static class RegisterServices
    {
        public static IServiceCollection ConfigureFirebaseAuthentication(this IServiceCollection services, string firebaseIdentification)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = $"https://securetoken.google.com/{firebaseIdentification}";
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = $"https://securetoken.google.com/{firebaseIdentification}",
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidAudience = firebaseIdentification,
                            ValidateLifetime = true
                        };
                    });

            return services;
        }
    }
}
